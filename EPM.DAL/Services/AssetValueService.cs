namespace DBSoft.EPM.DAL.Services
{
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using Aspects;
	using CodeFirst.Models;
	using DbSoft.Cache.Aspect.Attributes;
	using DTOs;
	using Enums;
	using Interfaces;
	using Market;
	using Materials;
	using Requests;

	/// <summary>
	/// Similar to AssetService but includes the aggregated value of the items
	/// </summary>
	public class AssetValueService : IAssetValueService
	{
		private readonly IAssetService _assets;
		private readonly IMarketPriceService _prices;
		private readonly IConfigurationService _config;
        private readonly IUniverseService _universe;
	    private readonly IMaterialItemService _materials;

	    public AssetValueService(IConfigurationService config, IAssetService assets, IMarketPriceService prices, 
            IUniverseService universe, IMaterialItemService materials)
		{
            _config = config;
            _assets = assets;
            _prices = prices;
            _universe = universe;
		    _materials = materials;
		}

        [Trace,Cache.Cacheable]
        public IEnumerable<AssetValueByStationDTO> ListMaterialsByStation(AssetValueRequest request)
		{
            ValidateRequest(request);
	   
            var result = from asset in GetAssets(request)
						 join price in _prices.List(new MarketPriceRequest { Token = request.Token, OrderType = OrderType.Buy }) on asset.ItemID equals price.ItemID
                         join material in _materials.ListBuildable(request.Token) on asset.ItemID equals material.ItemId
						 orderby asset.StationID
						 group new { asset, price } by new { asset.StationID, asset.StationName } into grp
						 select new AssetValueByStationDTO
						 {
							 StationID = grp.Key.StationID,
							 StationName = grp.Key.StationName,
							 Value = grp.Sum(f => f.asset.Quantity * f.price.CurrentPrice)
						 };
			return result.ToList();
		}

        [Trace,Cache.Cacheable]
        public List<AssetValueByStationAndItemDTO> ListByItemAndStation(AssetValueRequest request)
		{
            ValidateRequest(request);

            var prices = _prices.List(new MarketPriceRequest { Token = request.Token, OrderType = request.OrderType });
            var result = from asset in GetAssets(request)
						 join price in prices on asset.ItemID equals price.ItemID
						 orderby asset.StationID
						 group new { asset, price } by new { asset.StationID, asset.StationName, asset.ItemID, asset.ItemName } into grp
						 select new AssetValueByStationAndItemDTO
						 {
							 StationID = grp.Key.StationID,
							 StationName = grp.Key.StationName,
							 ItemID = grp.Key.ItemID,
							 ItemName = grp.Key.ItemName,
							 Value = grp.Sum(f => f.asset.Quantity * f.price.CurrentPrice)
                             
						 };
			return result.ToList();
		}

        public void ValidateRequest(AssetValueRequest request)
        {
            Contract.Requires(request != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(request.Token));
        }

        private IEnumerable<AssetByItemAndStationDTO> GetAssets(AssetValueRequest request)
        {
            var factory =
                _universe.GetStationSolarSystem(_config.GetSetting<int>(request.Token, ConfigurationType.FactoryLocation))
                    .RegionID;
            var assetRequest = new AssetServiceRequest
            {
                Token = request.Token,
                RegionID = factory
            };
            var assets = _assets.ListByItemAndStation(assetRequest).ToList();
            var market =
                _universe.GetStationSolarSystem(_config.GetSetting<int>(request.Token, ConfigurationType.MarketSellLocation))
                    .RegionID;
            if (factory != market)
            {
                assets.AddRange(_assets.ListByItemAndStation(new AssetServiceRequest
                {
                    Token = request.Token,
                    RegionID = market
                }));
            }
            return assets;
        }


	}
}
