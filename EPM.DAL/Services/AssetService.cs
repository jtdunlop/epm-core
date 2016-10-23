namespace DBSoft.EPM.DAL.Services
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Aspects;
    using AutoMapper;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Interfaces;
    using Queries;
    using Requests;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class AssetService : IAssetService
    {
        private readonly IDbContextFactory _factory;
        private readonly IUserService _users;

        public AssetService(IDbContextFactory factory, IUserService users)
        {
            _factory = factory;
            _users = users;
        }

        [Trace,Cache.Cacheable]
        public IEnumerable<AssetByItemAndStationDTO> ListByItemAndStation(AssetServiceRequest request)
        {
            ValidateRequest(request);
            var userID = _users.GetUserID(request.Token);
            using ( var context = _factory.CreateContext())
            {
                var assets = new AssetQuery(context);

                ApplySpecifications(request, assets);

                var result = from asset in assets.GetQuery()
                             where asset.UserID == userID
                             group asset by new { asset.ItemID, ItemName = asset.Item.Name, asset.StationID, StationName = asset.Station.Name } into grp
                             select new AssetByItemAndStationDTO
                             {
                                 ItemID = grp.Key.ItemID,
                                 ItemName = grp.Key.ItemName,
                                 StationID = grp.Key.StationID,
                                 StationName = grp.Key.StationName,
                                 Quantity = grp.Sum(f => (long)f.Quantity)
                             };

                return result.ToList();
            }
        }

        private static void ValidateRequest(AssetServiceRequest request)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(request.Token));
        }

        [Trace,Cache.Cacheable]
        public List<AssetByItemDTO> ListByItem(AssetServiceRequest request)
        {
            ValidateRequest(request);

            using ( var context = _factory.CreateContext() )
            {
                var assets = new AssetQuery(context);
                ApplySpecifications(request, assets);

                var result = from asset in assets.GetQuery()
                             group asset by new { asset.ItemID, ItemName = asset.Item.Name } into grp
                             select new AssetByItemDTO
                             {
                                 ItemID = grp.Key.ItemID,
                                 ItemName = grp.Key.ItemName,
                                 Quantity = grp.Sum(f => (long)f.Quantity)
                             };
                return result.ToList();
            }
        }

        public async Task UpdateAssets(List<SaveAssetRequest> requests)
        {
            ValidateRequests(requests);

            using ( var context = _factory.CreateContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                var saveAssetRequest = requests.FirstOrDefault();
                if (saveAssetRequest == null) return;
                var userID = _users.GetUserID(saveAssetRequest.Token);
                var allAssets = await context.Set<Asset>().ToListAsync();
                var myAssets = allAssets.Where(f => f.UserID == userID);
                foreach (var asset in myAssets.Where(f => requests.All(g => g.AssetID != f.ID)))
                {
                    asset.DeletedFlag = true;
                }
                foreach (var request in requests.OrderBy(f => f.ItemID))
                {
                    SaveAsset(context, request, allAssets);
                }
                context.ChangeTracker.DetectChanges();
                await context.SaveChangesAsync();
            }
        }

        private static void ValidateRequests(List<SaveAssetRequest> requests)
        {
            Contract.Requires(requests.Any());
            Contract.Requires(requests.All(f => !string.IsNullOrWhiteSpace(f.Token)));
        }

        private void SaveAsset(DbContext context, SaveAssetRequest request, ICollection<Asset> allAssets)
        {
            var userId = _users.GetUserID(request.Token);
            var asset = allAssets.SingleOrDefault(f => f.ID == request.AssetID) ?? new Asset();
            if (asset.ID == 0)
            {
                context.Set<Asset>().Add(asset);
                allAssets.Add(asset);
            }
            if (AreEquivalent(request, asset, userId))
            {
                return;
            }
            Mapper.Map(request, asset);
            asset.UserID = userId;
            asset.DeletedFlag = false;
        }

        private static bool AreEquivalent(SaveAssetRequest request, Asset asset, int userId)
        {
            return request.Quantity == asset.Quantity &&
                asset.UserID == userId &&
                request.LocationID == asset.LocationID &&
                asset.DeletedFlag == false;
        }

        private void ApplySpecifications(AssetServiceRequest request, AssetQuery assets)
        {
            var userId = _users.GetUserID(request.Token);
            assets.Specify(f => f.UserID == userId);

            if (request.StationID.HasValue)
            {
                assets.SpecifyStation(request.StationID.Value);
            }
            if (request.RegionID.HasValue)
            {
                assets.SpecifyRegion(request.RegionID.Value);
            }
            if ( request.SolarSystemID.HasValue)
            {
                assets.SpecifySolarSystem(request.SolarSystemID.Value);
            }
        }
    }
}
