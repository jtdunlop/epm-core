namespace DBSoft.EPM.DAL.Services
{
    using System;
    using Aspects;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
	using Enums;
	using Interfaces;
    using Market;
    using Requests;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
    using JetBrains.Annotations;
    using Transactions;

    [UsedImplicitly]
    public class MaterialPurchaseService : IMaterialPurchaseService
    {
		private readonly IAssetService _assets;
		private readonly IItemTransactionService _transactions;
		private readonly IMarketPriceService _prices;
		private readonly IConfigurationService _config;
		private readonly IMarketOrderService _orders;
		private readonly IMaterialRequirementService _materials;
		private readonly IItemService _buildable;
		private readonly IProductionMaterialService _productionMaterials;
        private readonly IUniverseService _universe;

		public MaterialPurchaseService(IAssetService assets, IItemTransactionService transactions, IMarketPriceService prices,
            IConfigurationService config, IMarketOrderService orders, IMaterialRequirementService materials, IItemService buildable,
            IProductionMaterialService productionMaterials, IUniverseService universe)
		{
            _assets = assets;
            _transactions = transactions;
            _prices = prices;
            _config = config;
            _orders = orders;
            _materials = materials;
            _buildable = buildable;
			_productionMaterials = productionMaterials;
            _universe = universe;
		}

		/// <summary>
		/// A list of materials that need to be purchased based on sales and inventory levels
		/// </summary>
		[Trace,Cache.Cacheable]
		public IEnumerable<MaterialPurchaseDto> List(MaterialPurchaseRequest request)
		{
			ValidateRequest(request);

			var token = request.Token;
			var transactions = _transactions.ListByItem(new ItemTransactionRequest 
			{ 
				Token = token, 
				DateRange = new DateRange(),
				TransactionType = TransactionType.Sell
			});
			var assets = GetAssets(token);
            var factoryAssets = GetFactoryAssets(token);
			var prices = _prices.List(new MarketPriceRequest { Token = request.Token, OrderType = OrderType.Buy });
            var rangePrices = _prices.List(new MarketPriceRequest
            {
                Token = request.Token,
                OrderType = OrderType.Buy,
                Range = _config.GetSetting<int>(token, ConfigurationType.BuyOrderRange),
                DateRange = new DateRange(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow)
            });
            var sellPrices = _prices.List(new MarketPriceRequest
            {
                Token = request.Token,
                OrderType = OrderType.Sell,
            });
            var orderRequest = CreateOrderRequest(token);
			var orders = _orders.List(orderRequest);
			var materials = _materials.ListBuildable(token);
			var buildables = _buildable.ListBuildable(token);
			var productions = _productionMaterials.List(token);

			var activeOrders = request.IncludeMaterialsWithActiveOrders;
			var overcut = activeOrders.HasValue && activeOrders.Value ? _config.GetSetting<decimal>(token, ConfigurationType.OvercutPercent) : 0;

			// Get a list of materials regardless if I have any, or have sold or am building anything that consumes them, or if any are at the hub
			var result = from material in materials
				   from asset in assets.Where(f => f.ItemID == material.MaterialId).DefaultIfEmpty()
                   from built in assets.Where(f => f.ItemID == material.ItemId).DefaultIfEmpty()
                   from factoryAsset in factoryAssets.Where(f => f.ItemID == material.MaterialId).DefaultIfEmpty()
				   from transaction in transactions.Where(f => f.ItemId == material.ItemId).DefaultIfEmpty()
				   from production in productions.Where(f => f.ItemId == material.MaterialId).DefaultIfEmpty()
				   from rangePrice in rangePrices.Where(f => f.ItemID == material.MaterialId).DefaultIfEmpty()
                   from sellPrice in sellPrices.Where(f => f.ItemID == material.MaterialId).DefaultIfEmpty()
                   from price in prices.Where(f => f.ItemID == material.MaterialId).DefaultIfEmpty()
				   // Must be buildable and have market price information available
				   join buildable in buildables on material.ItemId equals buildable.ItemID
				   where ShouldShowMaterial(activeOrders, orders, material)
				   group new { material, price, range = rangePrice, sale = transaction, asset, built, buildable, sellPrice }
					   by new
					   {
						   MaterialID = material.MaterialId,
						   material.MaterialName,
						   material.BounceFactor,
						   buildable.QuantityMultiplier,
						   RangePrice = rangePrice?.CurrentPrice ?? (price?.CurrentPrice ?? 0),
                           SellPrice = sellPrice?.CurrentPrice ?? 0,
						   MarketPrice = price?.CurrentPrice ?? 0,
						   Quantity = asset?.Quantity ?? 0,
						   Inventory = factoryAsset?.Quantity ?? 0,
						   Required = production?.Required ?? 0
					   } into grp
				   select new MaterialPurchaseDto
				   {
					   OvercutPercent = overcut,
					   ItemId = grp.Key.MaterialID,
					   ItemName = grp.Key.MaterialName,
					   MarketPrice = grp.Key.MarketPrice,
					   RangePrice = grp.Key.RangePrice,
                       SellPrice = grp.Key.SellPrice,
					   UsageQuantity = grp.Sum(f => GetUsageQuantity(f.sale, f.buildable, f.built) * (f.material.Quantity + f.material.AdditionalQuantity) / grp.Key.QuantityMultiplier),
					   InventoryQuantity = grp.Key.Quantity,
					   BounceFactor = grp.Key.BounceFactor,
					   FactoryQuantity = grp.Key.Inventory,
					   FactoryRequired = grp.Key.Required
				   };
            return result.Where(f => f.PurchaseQuantity > 0);
		}

        private static void ValidateRequest(MaterialPurchaseRequest request)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(request.Token));
        }

        private MarketOrderRequest CreateOrderRequest(string token)
        {
            var station = _config.GetSetting<int>(token, ConfigurationType.MarketBuyLocation);
            if ( station == 0 )
                station = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
            var region = _universe.GetStationSolarSystem(station).RegionID;
            var orderRequest = new MarketOrderRequest
            {
                Token = token,
                RegionId = region
            };
            return orderRequest;
        }

        private IEnumerable<AssetByItemDTO> GetFactoryAssets(string token)
        {
            var factory = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
            var assets = _assets.ListByItem(new AssetServiceRequest
            {
                Token = token,
                StationID = factory
                
            }).ToList();
            return assets;
        }

        private IEnumerable<AssetByItemDTO> GetAssets(string token)
	    {
            var factory = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
            var factoryRegion = _universe.GetStationSolarSystem(factory).RegionID;
	        var assets = _assets.ListByItem(new AssetServiceRequest
	        {
	            Token = token,
                RegionID = factoryRegion
	        }).ToList();

            var market = _config.GetSetting<int>(token, ConfigurationType.MarketSellLocation);
            var marketRegion = _universe.GetStationSolarSystem(market).RegionID;
            if ( marketRegion != factoryRegion )
            {
                assets.AddRange(_assets.ListByItem(new AssetServiceRequest
                {
                    Token = token,
                    RegionID = marketRegion
                }));
            }
            var result = from asset in assets
                         group asset by new { asset.ItemID, asset.ItemName } into grp
                         select new AssetByItemDTO
                         {
                             ItemID = grp.Key.ItemID,
                             ItemName = grp.Key.ItemName,
                             Quantity = grp.Sum(f => f.Quantity)
                         };
            return result;
	    }

	    private static bool ShouldShowMaterial(bool? activeOrders, IEnumerable<MarketOrderDTO> orders, MaterialRequirementDto material)
		{
			var result = activeOrders == null || (activeOrders.Value ? orders.Any(f => f.ItemID == material.MaterialId) : orders.All(f => f.ItemID != material.MaterialId));
			return result;
		}

		private static long GetUsageQuantity(ItemTransactionBaseDto sale, BuildableItemDTO buildable, AssetBaseDTO built)
		{
		    // If an item hasn't sold in the past week (or ever) still need to reserve enough inventory to kick off the next build. Otherwise, 
			// can't jumpstart a new item, and in rare cases a change in minimum quantity could strand the item.
		    if (sale != null && sale.Quantity != 0) return sale.Quantity;
		    // Special case. When no sales build to enough materials to build two batches which is four half batch purchases
		    // QuantityMultiplier is getting factored in because it gets factored out later.
		    var qty = built?.Quantity ?? 0;
		    return Math.Max((buildable.MinimumStock.GetValueOrDefault() - qty) * buildable.QuantityMultiplier * 2, 0);
		}
	}
}
