namespace DBSoft.EPM.Logic.RefreshApi
{
	using System;
	using System.Threading.Tasks;
	using DAL.Annotations;
	using DAL.DTOs;
	using DAL.Interfaces;
	using DAL.Requests;
	using System.Collections.Generic;
	using System.Linq;
	using DAL.Services;
	using Asset = EVEAPI.Entities.Asset.Asset;
	using AssetService = DAL.Services.AssetService;
    using DAL.Services.AccountApi;

	public static class CategoryType
	{
		public const int Blueprint = 9;
	}

    [UsedImplicitly]
    public class AssetMapper : EveApiMapper, IAssetMapper
    {
        private readonly IAssetService _assetService;
		private readonly IItemService _itemService;
        private readonly EVEAPI.Entities.Asset.IAssetService _assets;
        private IEnumerable<ItemDTO> _items;
	    private readonly IUniverseService _universe;
        private readonly IAccountApiService _accounts;

        public AssetMapper(IDbContextFactory factory, IEveApiStatusService statusService, IUniverseService universe,
            IAccountApiService accounts, IItemService items, EVEAPI.Entities.Asset.IAssetService assets, IUserService users)
            : base(statusService)
        {
            _assetService = new AssetService(factory, users);
            _itemService = items;
            _assets = assets;
            _universe = universe;
            _accounts = accounts;
        }

        public async Task Pull(string token)
		{
			_items = _itemService.List(new ItemRequest());

			var cachedUntil = DateTime.Now;
			const string serviceName = "AssetList";
			var accounts = _accounts.List(token);
		    var assetQueue = new List<SaveAssetRequest>();
            var blueprints = _itemService.List(new ItemRequest
            {
                CategoryId = CategoryType.Blueprint
            });
            try
            {
                foreach (var account in accounts)
                {
                    var response = await _assets.Load(account.ApiKeyType, account.ApiKeyID, account.ApiVerificationCode, account.EveApiID);
                    ProcessAssets(response.Data, account.AccountID, blueprints, token, assetQueue);
                    cachedUntil = response.CachedUntil;
                }
                await _assetService.UpdateAssets(assetQueue);
            }
            catch ( Exception e )
            {
                SaveError(serviceName, token, e.Message);
                throw;
            }

			UpdateStatus(serviceName, cachedUntil, token);
		}

		private static bool IsBlueprint(IEnumerable<ItemDTO> blueprints, Asset asset )
		{
			return blueprints.Any(f => f.ItemID == asset.ItemID);
		}

		private void ProcessAssets(IEnumerable<Asset> eveAssets, int accountId, List<ItemDTO> blueprints, string token, 
            ICollection<SaveAssetRequest> assetQueue)
		{
            var stations = _universe.ListStations().ToList();
            var systems = _universe.ListSolarSystems().ToList();

		    foreach (var eveAsset in eveAssets)
			{
				var isBlueprint = IsBlueprint(blueprints, eveAsset);
				if (!eveAsset.IsPackaged && !isBlueprint) continue;
				if (eveAsset.LocationID == null) continue;
				var locationId = eveAsset.LocationID.Value;
				// Hangar flags are packed in there
				if (locationId >= 66000000 && locationId < 67000000)
				{
					locationId -= 6000001;
				}
                if (!IsStation(stations, locationId) && !IsSolarSystem(systems, locationId)) continue;
                if (!GetItem(eveAsset.ItemID)) continue;
			    assetQueue.Add(QueueAsset(accountId, locationId, eveAsset, token));
			}
		}

	    private static bool IsSolarSystem(IEnumerable<SolarSystemDTO> systems, int locationId)
	    {
            return systems.Any(f => f.SolarSystemID == locationId);
	    }

	    private static bool IsStation(IEnumerable<StationDTO> stations, int? stationId)
        {
            return stationId.HasValue && stations.Any(f => f.StationID == stationId);
        }

		private static SaveAssetRequest QueueAsset(int accountId, int locationId, Asset asset, string token)
		{
			var request = new SaveAssetRequest
				{
					Token = token,
					AccountID = accountId,
					AssetID = asset.ID,
					ItemID = asset.ItemID,
					LocationID = locationId,
					Quantity = asset.Quantity
				};
			return request;
		}

		private bool GetItem(int itemId)
		{
			return _items.Any(f => f.ItemID == itemId);
		}
	}
}