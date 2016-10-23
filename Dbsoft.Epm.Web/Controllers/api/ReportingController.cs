using Microsoft.AspNetCore.Mvc;

namespace Dbsoft.Epm.Web.Controllers.api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DBSoft.EPM.DAL;
    using DBSoft.EPM.DAL.CodeFirst.Models;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Services;
    using DBSoft.EPM.DAL.Services.ItemCosts;
    using DBSoft.EPM.DAL.Services.MaterialCosts;
    using DBSoft.EPM.DAL.Services.Transactions;
    using Reporting.DailySales;

    [Route("api/[controller]")]
    public class ReportingController : EpmController
    {
        private readonly IItemTransactionService _sales;
        private readonly IItemCostService _costs;
        private readonly IMaterialCostService _materials;

        public ReportingController(IItemTransactionService sales, IUserService users, IItemCostService costs,
            IMaterialCostService materials) : base(users)
        {
            _sales = sales;
            _costs = costs;
            _materials = materials;
        }

        [Route("ItemSales")]
        public IEnumerable<ItemTransactionByItemDto> ItemSales(DateTime fromDate, DateTime toDate, int? itemId)
        {
            var result = _sales.ListByItem(new ItemTransactionRequest
            {
                Token = Token,
                DateRange = new DateRange(fromDate, toDate),
                TransactionType = TransactionType.Sell,
                ItemID = itemId
            }).OrderByDescending(o => o.GpAmt);
            return result;
        }

        [Route("SubscriberSales")]
        public IEnumerable<ItemTransactionBySubscriberDto> SubscriberSales(DateTime fromDate, DateTime toDate)
        {
            var result = _sales.ListBySubscriber(new SubscriberTransactionRequest
            {
                Token = Token,
                DateRange = new DateRange(fromDate, toDate),
                TransactionType = TransactionType.Sell
            });
            return result;
        }

        [Route("itemsalesdetail")]
        public IEnumerable<ItemTransactionDto> ItemSalesDetail(DateTime fromDate, DateTime toDate, int? itemId)
        {
            var result = _sales.List(new ItemTransactionRequest
            {
                Token = Token,
                DateRange = new DateRange(fromDate, toDate),
                TransactionType = TransactionType.Sell,
                ItemID = itemId
            }).OrderBy(o => o.DateTime);
            return result;
        }

        [Route("DailySales")]
        public IEnumerable<DailySaleItemModel> DailySales()
        {
            var result = Mapper.Map<IEnumerable<ItemTransactionByDateDto>, IEnumerable<DailySaleItemModel>>(_sales
                .ListByDate(new ItemTransactionRequest
                {
                    Token = Token,
                    DateRange = new DateRange(),
                    TransactionType = TransactionType.Sell
                })
                .OrderByDescending(o => o.DateTime));
            
            return result;
        }

        [Route("MonthlySales")]
        public IEnumerable<ItemTransactionByMonthDto> MonthlySales()
        {
            var result = _sales
                .ListByMonth(new ItemTransactionRequest
                {
                    Token = Token,
                    DateRange = new DateRange(DateTime.Now.AddMonths(-12).StartOfTheMonth(), DateTime.Now.AddMonths(-1).EndOfTheMonth()),
                    TransactionType = TransactionType.Sell
                })
                .OrderByDescending(o => o.DateTime);
            return result;
        }

        [Route("ItemCostTrends")]
        public IEnumerable<ItemCostVarianceDto> ItemCostTrends()
        {
            var result = _costs.ListVariances(Token).OrderByDescending(f => f.Variance);
            return result;
        }

        [Route("MaterialCostTrends")]
        public IEnumerable<MaterialCostVarianceDto> MaterialCostTrends()
        {
            var result = _materials.ListVariances(Token).OrderByDescending(f => f.Variance);
            return result;
        }

        [Route("ItemMaterialCostTrends")]
        public IEnumerable<MaterialCostVarianceByItemDto> ItemMaterialCostTrends(int itemId)
        {
            var result =
                _costs.ListVariancesByItem(Token)
                    .Where(f => f.ItemId == itemId)
                    .OrderByDescending(o => o.WeightedVariance);
            return result;
        }
    }
}
