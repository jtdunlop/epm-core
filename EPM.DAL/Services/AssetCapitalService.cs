namespace DBSoft.EPM.DAL.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Aspects;
	using CodeFirst.Models;
	using DbSoft.Cache.Aspect.Attributes;
	using DTOs;
	using Interfaces;
	using Enums;
	using Market;
	using Materials;
	using ProductionJobs;
	using Requests;

	public class AssetCapitalService : IAssetCapitalService
	{
		private readonly IItemService _items;
		private readonly IMaterialItemService _materials;
		private readonly IAssetValueService _assets;
		private readonly IConfigurationService _config;
		private readonly IMarketOrderService _orders;
		private readonly IProductionJobService _jobs;
		private readonly IMarketPriceService _prices;
	    private readonly IUniverseService _universe;

	    public AssetCapitalService(IItemService items, IMaterialItemService materials, IAssetValueService assets, IConfigurationService config, 
			IMarketOrderService orders, IProductionJobService jobs, IMarketPriceService prices, IUniverseService universe)
		{
			_items = items;
			_materials = materials;
			_assets = assets;
			_config = config;
			_orders = orders;
			_jobs = jobs;
			_prices = prices;
	        _universe = universe;
		}

		[Trace,Cache.Cacheable]
		public IEnumerable<MaterialCapitalDTO> ListMaterialCapital(string token)
		{
			var materials = _materials.ListBuildable(token);
			var assets = _assets.ListByItemAndStation(new AssetValueRequest
			{
                OrderType = OrderType.Buy,
				Token = token
			}).Where(f => materials.Any(g => g.ItemId == f.ItemID)).ToList();
			var factory = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
			var localAssets = from asset in assets.Where(f => f.StationID == factory)
							  group new { asset } by new { asset.ItemID, asset.ItemName } into grp
							  select new
							  {
								  grp.Key.ItemID,
								  grp.Key.ItemName,
								  Value = grp.Sum(f => f.asset.Value)
							  };
			var remoteAssets = from asset in assets.Where(f => f.StationID != factory)
							   group new { asset } by new { asset.ItemID, asset.ItemName } into grp
							   select new
							   {
								   grp.Key.ItemID,
								   grp.Key.ItemName,
								   Value = grp.Sum(f => f.asset.Value)
							   };

            var orderRequest = CreateOrderRequest(token, ConfigurationType.FactoryLocation);
            var orders = _orders.List(orderRequest);
			var result = from material in materials
						 from local in localAssets.Where(f => f.ItemID == material.ItemId).DefaultIfEmpty()
						 from remote in remoteAssets.Where(f => f.ItemID == material.ItemId).DefaultIfEmpty()
						 from order in orders.Where(f => f.ItemID == material.ItemId).DefaultIfEmpty()
						 select new MaterialCapitalDTO
						 {
							 ItemID = material.ItemId,
							 ItemName = material.ItemName,
							 FactoryValue = local == null ? 0 : local.Value,
							 RemoteValue = remote == null ? 0 : remote.Value,
							 MarketValue = order == null ? 0 : order.Escrow
						 };
			var r = result.ToList();
            return r;
		}

	    private MarketOrderRequest CreateOrderRequest(string token, ConfigurationType type)
	    {
            var station = _config.GetSetting<int>(token, type);
            var region = _universe.GetStationSolarSystem(station).RegionID;
            var orderRequest = new MarketOrderRequest
            {
                Token = token,
                RegionId = region
            };
            return orderRequest;
        }

	    [Trace,Cache.Cacheable]
		public IEnumerable<ItemCapitalDTO> ListItemCapital(string token)
		{
            var jobs = _jobs.List(new ProductionJobRequest
            {
                Token = token,
                Status = ProductionJobStatus.Active
            });
            var jobAssets = from job in jobs
                            join price in _prices.List(new MarketPriceRequest
                            {
                                Token = token,
                                OrderType = OrderType.Sell
                            }) on job.ItemID equals price.ItemID
                            select new
                            {
                                job.ItemID,
                                job.ItemName,
                                job.Quantity,
                                price.CurrentPrice
                            };
			var items = _items.ListBuildable(token);
			var assets = _assets.ListByItemAndStation(new AssetValueRequest
			{
                OrderType = OrderType.Sell,
				Token = token
			}).Where(f => items.Any(g => g.ItemID == f.ItemID)).ToList();

            var orderRequest = CreateOrderRequest(token, ConfigurationType.MarketSellLocation);
            var orders = _orders.List(orderRequest);
			var factory = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
			var market = _config.GetSetting<int>(token, ConfigurationType.MarketSellLocation);
			var result = from material in items
						 from local in assets.Where(f => f.StationID == factory && f.ItemID == material.ItemID).DefaultIfEmpty()
						 from remote in assets.Where(f => f.StationID == market && f.ItemID == material.ItemID).DefaultIfEmpty()
						 from order in orders.Where(f => f.ItemID == material.ItemID).DefaultIfEmpty()
						 from job in jobAssets.Where(f => f.ItemID == material.ItemID).DefaultIfEmpty()
						 select new ItemCapitalDTO
						 {
							 ItemID = material.ItemID,
							 ItemName = material.ItemName,
							 FactoryValue = local == null ? 0 : local.Value,
							 RemoteValue = remote == null ? 0 : remote.Value,
							 MarketValue = order == null ? 0 : order.Escrow,
							 JobValue = job == null ? 0 : job.Quantity * job.CurrentPrice
						 };
			var r = result.ToList();
            return r;
		}

	}
}
