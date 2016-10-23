namespace DBSoft.EPM.DAL.Services
{
	using System.Diagnostics.Contracts;
	using CacheAspect.Attributes;
	using DTOs;
	using Interfaces;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Models;
	using Requests;

	public class ItemInventoryService
	{
		private readonly AssetService _assets;
		private readonly ItemSaleService _sales;
		private readonly MarketPriceService _prices;
		private readonly MarketPriceService _rangePrices;
		private readonly ConfigurationService _config;
		private readonly MaterialUsageService _materials;
		private readonly MarketOrderService _orders;

		public ItemInventoryService(IDbContextFactory factory)
		{
			_assets = new AssetService(factory);
			_sales = new ItemSaleService(factory);
			_prices = new MarketPriceService(factory);
			_config = new ConfigurationService(factory);
			_materials = new MaterialUsageService(factory);
			_orders = new MarketOrderService(factory);
			_rangePrices = new MarketPriceService(factory);
		}

		[Cache.Cacheable]
		public IEnumerable<ItemInventoryDTO> List(string token)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(token));

			var materials = _materials.List(token);
			var sales = _sales.ListByItem(new ItemSaleListRequest { Token = token, DateRange = new DateRange() });
			var assets = _assets.ListByItem(new AssetServiceRequest { Token = token, RegionID = _config.GetSetting<int>(token, ConfigurationType.BusinessRegion) });
			var prices = _prices.List(new MarketPriceServiceRequest { OrderType = OrderType.Buy});
			var rangePrices = _rangePrices.List(new MarketPriceServiceRequest { OrderType = OrderType.Buy, Range = _config.GetSetting<int>(token, ConfigurationType.BuyOrderRange) });
			var orders = _orders.List(token);

			var result = from asset in assets
						 join material in materials on asset.ItemID equals material.MaterialID
						 join sale in sales on material.ItemID equals sale.ItemID
						 join price in prices on material.MaterialID equals price.ItemID
						 join rangePrice in rangePrices on material.MaterialID equals rangePrice.ItemID
						 where orders.All(f => f.ItemID != asset.ItemID)
						 orderby material.MaterialID
						 group new { material, price, range = rangePrice, sale, asset } 
							by new 
							{ 
								material.MaterialID, 
								material.MaterialName, 
								material.BounceFactor, 
								MarketPrice = price.CurrentPrice, 
								RangePrice = rangePrice.CurrentPrice, 
								asset.Quantity 
							} into grp
						 select new ItemInventoryDTO
						 {
							 ItemID = grp.Key.MaterialID,
							 ItemName = grp.Key.MaterialName,
							 MarketPrice = grp.Key.MarketPrice,
							 RangePrice = grp.Key.RangePrice,
							 UsageQuantity = grp.Sum(f => f.sale.Quantity * (f.material.Quantity + f.material.AdditionalQuantity)),
							 InventoryQuantity = grp.Key.Quantity,
							 BounceFactor = grp.Key.BounceFactor
						 };
			return result.ToList();
		}
	}
}
