namespace DBSoft.EPM.DAL.Services
{
    using Aspects;
    using DbSoft.Cache.Aspect.Attributes;
    using ItemCosts;
    using Market;
    using Queries;
	using CodeFirst.Models;
	using DTOs;
	using Enums;
	using Interfaces;
	using Requests;
	using System;
	using System.Collections.Generic;
	using System.Linq;
    using JetBrains.Annotations;
    using Transactions;

    [UsedImplicitly]
    public class ProductionQueueService : IProductionQueueService
    {
		private readonly IItemTransactionService _transactions;
		private readonly IItemCostService _costs;
        private readonly IItemPriceService _itemPrices;
        private readonly IAssetService _inventory;
		private readonly IMarketPriceService _prices;
		private readonly IConfigurationService _config;
		private readonly IBlueprintInstanceService _instances;
		private readonly IItemService _items;
        private readonly IUniverseService _universe;
        private readonly IMarketOrderService _orders;
        private readonly IMarketResearchService _research;
        private readonly IUserService _users;
        private readonly DataQuery<ProductionJob> _building;

        public ProductionQueueService(IDbContextFactory factory, IItemCostService costs, IItemPriceService itemPrices,
            IItemTransactionService transactions, IConfigurationService config, IAssetService inventory, IMarketPriceService prices,
            IBlueprintInstanceService instances, IItemService items, IUniverseService universe, IMarketOrderService orders,
            IMarketResearchService research, IUserService users)
		{
            _transactions = transactions;
			_config = config;
			_inventory = inventory;
			_prices = prices;
			_instances = instances;
			_items = items;
            _universe = universe;
            _orders = orders;
            _research = research;
            _users = users;
            _costs = costs;
		    _itemPrices = itemPrices;
            var context = factory.CreateContext();
            _building = new DataQuery<ProductionJob>(context);
		}

		[Trace,Cache.Cacheable]
		public IEnumerable<ProductionQueueDto> List(string token)
		{
			var items = _items.ListBuildable(token);

            var user = _users.GetUserID(token);
            var building = _building
                .Specify(f => f.Status == ProductionJobStatus.Active)
                .Specify(f => f.Asset.UserID == user)
                .GetQuery()
                .ToList(); 

            var transactions = _transactions.ListByItem(new ItemTransactionRequest
            {
                Token = token,
                DateRange = new DateRange(),
                TransactionType = TransactionType.Sell
            });

			var system = _universe.GetStationSolarSystem(_config.GetSetting<int>(token, ConfigurationType.FactoryLocation));
			var inventory = GetInventory(token, system);
            var instances = _instances.ListBuildable(token);
            var distinctInstances = _instances.ListDistinctBuildable(token);
			var prices = _prices.List(new MarketPriceRequest { Token = token, OrderType = OrderType.Sell });
            var itemPrices = _itemPrices.ListBuildable(token);
            var research = _research.List(token);
            var minimumMarkup = _config.GetSetting<decimal>(token, ConfigurationType.MinimumMarkup);

			var costs = _costs.ListBuildable(new ListBuildableRequest 
			{ 
				Token = token,
				Date = DateTime.Now 
			});
            UpdateCosts(research, costs);

		    var result = GetResult(items, distinctInstances, building, inventory, transactions, costs, prices, itemPrices,
		        research, instances, minimumMarkup);

			return result;
		}

        [Trace]
        private static List<ProductionQueueDto> GetResult(IEnumerable<BuildableItemDTO> items, IEnumerable<BlueprintInstanceDto> distinctInstances, 
            IEnumerable<ProductionJob> building, IEnumerable<AssetByItemDTO> inventory,
            IEnumerable<ItemTransactionByItemDto> transactions, IEnumerable<ItemCostDTO> costs, IEnumerable<MarketPriceDTO> prices, 
            IEnumerable<ItemPriceDTO> itemPrices, IEnumerable<MarketResearchDTO> research, IEnumerable<BlueprintInstanceDto> instances,
            decimal minimumMarkup)
        {
            var result = from item in items
                         join instance in distinctInstances on item.ItemID equals instance.BuildItemID
                         from b in building.Where(f => f.ID == item.ItemID).DefaultIfEmpty()
                         from i in inventory.Where(f => f.ItemID == item.ItemID).DefaultIfEmpty()
                         from transaction in transactions.Where(f => f.ItemId == item.ItemID).DefaultIfEmpty()
                         from cost in costs.Where(f => f.ItemID == item.ItemID).DefaultIfEmpty()
                         from price in prices.Where(f => f.ItemID == item.ItemID).DefaultIfEmpty()
                         from ip in itemPrices.Where(f => f.ItemID == item.ItemID).DefaultIfEmpty()
                         from r in research.Where(f => f.ItemID == item.ItemID).DefaultIfEmpty()
                         select new ProductionQueueDto
                         {
                             ItemId = item.ItemID,
                             ItemName = item.ItemName,
                             Building = b?.Quantity ?? 0,
                             AvailableBlueprints = instances.Count(f => f.BuildItemID == item.ItemID),
                             Sold = transaction?.Quantity / item.QuantityMultiplier ?? 0,
                             Price = price?.CurrentPrice,
                             Inventory = i?.Quantity / item.QuantityMultiplier ?? 0,
                             MinimumStock = item.MinimumStock,
                             ProductionTime = instance.ProductionTime / item.QuantityMultiplier,
                             Cost = cost?.Cost ?? 0,
                             FreightCost = cost?.FreightCost ?? 0,
                             TimeEfficiency = item.ProductionEfficiency,
                             MinimumMarkup = ip == null ? 0 : ip.Variance + minimumMarkup,
                             ProfitFactor = r?.ProfitFactor
                         };
            return result.ToList();
        }

        [Trace]
        private static void UpdateCosts(IEnumerable<MarketResearchDTO> research, List<ItemCostDTO> costs)
        {
            foreach ( var r in research )
            {
                var cost = costs.SingleOrDefault(f => f.ItemID == r.ItemID);
                if ( cost != null )
                {
                    r.Cost = cost.Cost;
                }
            }
        }

        [Trace]
        private List<AssetByItemDTO> GetInventory(string token, SolarSystemDTO system)
        {
            var market = _config.GetSetting<int>(token, ConfigurationType.MarketSellLocation);
            var marketRegion = _universe.GetStationSolarSystem(market)
                .RegionID;
            var region = system.RegionID;
            var assetRequest = new AssetServiceRequest { Token = token, RegionID = region, OrderType = OrderType.Sell };
            var inventory = _inventory.ListByItem(assetRequest).ToList();
            if ( region != marketRegion )
            {
                var more = _inventory.ListByItem(new AssetServiceRequest
                {
                    Token = token, 
                    StationID = market,
                    OrderType = OrderType.Sell
                });
                inventory.AddRange(more);
            }

            var grouped = from i in inventory
                         group i by new { i.ItemID, i.ItemName } into grp
                         select new AssetByItemDTO
                         {
                             ItemID = grp.Key.ItemID,
                             ItemName = grp.Key.ItemName,
                             Quantity = grp.Sum(f => f.Quantity)
                         };

            var orders = _orders.List(new MarketOrderRequest
            {
                Token = token,
                RegionId = marketRegion
            });
            // Find items at market not on market. Don't count them in stock.
            var selling = _inventory.ListByItem(new AssetServiceRequest
            {
                StationID = market,
                Token = token,
                OrderType = OrderType.Sell
            }).Where(f => orders.All(o => o.ItemID != f.ItemID));

            var result = from i in grouped
                         from sell in selling.Where(f => f.ItemID == i.ItemID).DefaultIfEmpty()
                         select new AssetByItemDTO
                         {
                             ItemID = i.ItemID,
                             ItemName = i.ItemName,
                             Quantity = i.Quantity - (sell?.Quantity ?? 0)
                         };
            return result.ToList();
        }
    }
}
