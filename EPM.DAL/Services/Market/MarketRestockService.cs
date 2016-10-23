namespace DBSoft.EPM.DAL.Services.Market
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Aspects;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Enums;
    using Interfaces;
    using Requests;

    public class MarketRestockService : IMarketRestockService
    {
		private readonly IMarketOrderService _orders;
		private readonly IItemPriceService _prices;
		private readonly IConfigurationService _config;
		private readonly IAssetService _assets;
        private readonly IUniverseService _universe;

        public MarketRestockService(IMarketOrderService orders, IItemPriceService prices, IConfigurationService config, 
            IAssetService assets, IUniverseService universe)
		{
		    _orders = orders;
		    _prices = prices;
		    _config = config;
		    _assets = assets;
		    _universe = universe;
		}

	    [Trace,Cache.Cacheable]
		public IEnumerable<MarketRestockDTO> List(string token)
		{
			ValidateToken(token);

            var station = _config.GetSetting<int>(token, ConfigurationType.MarketSellLocation);
            var region = _universe.GetStationSolarSystem(station).RegionID;
            var orderRequest = new MarketOrderRequest
            {
                Token = token,
                RegionId = region
            };

            var orders = _orders.List(orderRequest);
			var prices = _prices.ListBuildable(token);
			var assets = _assets.ListByItem(new AssetServiceRequest
				{
					Token = token,
					StationID = _config.GetSetting<int>(token, ConfigurationType.MarketSellLocation)
				});
			var markup = _config.GetSetting<decimal>(token, ConfigurationType.MinimumMarkup);

			var result = from asset in assets
						 join price in prices on asset.ItemID equals price.ItemID
						 where orders.All(f => f.ItemID != asset.ItemID)
						 select new MarketRestockDTO
						 {
							 ItemID = asset.ItemID,
							 ItemName = asset.ItemName,
							 MarketPrice = price.CurrentPrice,
							 ItemCost = price.Cost,
							 Variance = price.Variance,
							 MinimumMarkup = markup,
                             Timestamp = price.Timestamp
						 };
			return result.ToList();
		}

        private static void ValidateToken(string token)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(token));
        }
    }
}
