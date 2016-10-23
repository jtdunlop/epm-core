namespace DBSoft.EPM.DAL.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Aspects;
	using CodeFirst.Models;
	using DbSoft.Cache.Aspect.Attributes;
	using DTOs;
	using Interfaces;
	using ItemCosts;
	using Market;
	using Requests;

    public class ItemPriceService : IItemPriceService
    {
		private readonly IItemCostService _costs;
		private readonly IMarketPriceService _prices;

		public ItemPriceService(IItemCostService costs, IMarketPriceService prices)
		{
		    _costs = costs;
		    _prices = prices;
		}

	    [Trace,Cache.Cacheable]
        public List<ItemPriceDTO> ListBuildable(string token)
		{
			var costs = _costs.ListBuildable(new ListBuildableRequest
				{
					Token = token,
					Date = DateTime.Today
				});
			var prices = _prices.List(new MarketPriceRequest
				{
					Token = token,
					OrderType = OrderType.Sell
				});
			var variances = _costs.ListVariances(token);

			var result = from cost in costs
						 join price in prices on cost.ItemID equals price.ItemID
						 join variance in variances on cost.ItemID equals variance.ItemId
						 select new ItemPriceDTO
						 {
							 ItemID = cost.ItemID,
							 ItemName = cost.ItemName,
							 CurrentPrice = price.CurrentPrice,
							 Cost = cost.Cost,
							 Variance = variance.Variance,
                             Timestamp = price.Timestamp
						 };
			return result.ToList();
		}
	}
}
