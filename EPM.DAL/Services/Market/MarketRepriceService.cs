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

    public class MarketRepriceService : IMarketRepriceService
    {
		private readonly IMarketOrderService _orders;
		private readonly IItemPriceService _prices;
		private readonly IConfigurationService _config;
        private readonly IUniverseService _universe;

        public MarketRepriceService(IMarketOrderService orders, IItemPriceService prices, IConfigurationService config,
            IUniverseService universe)
        {
		    _orders = orders;
		    _prices = prices;
		    _config = config;
		    _universe = universe;
		}

	    [Trace,Cache.Cacheable]
		public IEnumerable<MarketRepriceDTO> List(MarketRepriceRequest request)
		{
			ValidateRequest(request);

            var station = _config.GetSetting<int>(request.Token, ConfigurationType.MarketSellLocation);
            var region = _universe.GetStationSolarSystem(station).RegionID;
            var orderRequest = new MarketOrderRequest
            {
                Token = request.Token,
                RegionId = region
            };
            var orders = _orders.List(orderRequest);
			var prices = _prices.ListBuildable(request.Token);
			var undercut = _config.GetSetting<decimal>(request.Token, ConfigurationType.UndercutPercent);
			var markup = _config.GetSetting<decimal>(request.Token, ConfigurationType.MinimumMarkup);

			var result = from order in orders
						 join price in prices on order.ItemID equals price.ItemID
						 select new MarketRepriceDTO
						 {
							 ItemID = order.ItemID,
							 ItemName = order.ItemName,
							 MarketPrice = price.CurrentPrice,
							 ListedPrice = order.Price,
							 Cost = price.Cost,
							 Variance = price.Variance,
							 UndercutPercent = undercut,
							 MinimumMarkup = markup,
                             Timestamp = price.Timestamp
						 };
			return result.ToList();
		}

        private static void ValidateRequest(MarketRepriceRequest request)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(request.Token));
        }
    }
}
