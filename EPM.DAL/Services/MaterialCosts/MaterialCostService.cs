namespace DBSoft.EPM.DAL.Services.MaterialCosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aspects;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Interfaces;
    using JetBrains.Annotations;
    using Market;
    using Materials;
    using Requests;
    using Transactions;

    [UsedImplicitly]
    public class MaterialCostService : IMaterialCostService
    {
		private readonly IItemTransactionService _purchases;
		private readonly IMarketPriceService _prices;
        private readonly IFreightCalculator _calculator;
        private readonly IMaterialItemService _materials;

        public MaterialCostService(IDbContextFactory factory, IItemTransactionService purchases,
            IMarketPriceService prices, IFreightCalculator calculator, IMaterialItemService materials)
        {
            _purchases = purchases;
            _prices = prices;
            _calculator = calculator;
            _materials = materials;
        }

        [Trace,Cache.Cacheable]
		public List<MaterialCostDTO> List(MaterialCostRequest request)
        {
            var materials = _materials.ListBuildable(request.Token);

			var prices = _prices.List(new MarketPriceRequest {  Token = request.Token, OrderType = OrderType.Buy });

			var r = new ItemTransactionRequest
				{
					Token = request.Token,
					TransactionType = TransactionType.Buy
				};
			if ( request.Date.HasValue )
			{
				r.DateRange = new DateRange(request.Date.Value.AddDays(-30), request.Date.Value);
			}
			var purchases = _purchases.ListByItem(r);

            var result = from material in materials
                from purchase in purchases.Where(f => f.ItemId == material.ItemId).DefaultIfEmpty()
                from price in prices.Where(f => f.ItemID == material.ItemId).DefaultIfEmpty()
                select new MaterialCostDTO
                {
                    MaterialID = material.ItemId,
                    MaterialName = material.ItemName,
                    FreightCost = _calculator.GetFreightCost(request.Token, GetPrice(purchase, price), material.Volume),
                    HistoryPrice = purchase?.Price,
                    HubPrice = price?.CurrentPrice
                };

			return result.ToList();
		}

        [Trace, Cache.Cacheable]
        public IEnumerable<MaterialCostVarianceDto> ListVariances(string token)
        {
            var costNow = List(new MaterialCostRequest { Token = token, Date = DateTime.Now });
            var costThen = List(new MaterialCostRequest { Token = token, Date = DateTime.Now.AddDays(-7) });

            var result = from now in costNow
                         join then in costThen on now.MaterialID equals then.MaterialID
                         select new MaterialCostVarianceDto
                         {
                             MaterialId = now.MaterialID,
                             MaterialName = now.MaterialName,
                             CostNow = now.Cost,
                             CostThen = then.Cost
                         };

            return result.ToList();
        }

        private static decimal GetPrice(ItemTransactionBaseDto purchase, MarketPriceDTO price)
        {
            if ( purchase != null )
            {
                return purchase.Price;
            }
            return price?.CurrentPrice ?? 0;
        }
	}
}
