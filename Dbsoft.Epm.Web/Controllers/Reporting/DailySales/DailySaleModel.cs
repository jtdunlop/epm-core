namespace Dbsoft.Epm.Web.Controllers.Reporting.DailySales
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DBSoft.EPM.DAL;
    using DBSoft.EPM.DAL.CodeFirst.Models;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Services.Transactions;

    public class DailySaleModel
    {
        public DailySaleModel(IItemTransactionService service, string token)
        {
            Detail = Mapper.Map<IEnumerable<ItemTransactionByDateDto>, IEnumerable<DailySaleItemModel>>(service
                .ListByDate(new ItemTransactionRequest 
                { 
                    Token = token, 
                    DateRange = new DateRange(),
                    TransactionType = TransactionType.Sell
                })
                .OrderByDescending(o => o.DateTime));
        }
        public IEnumerable<DailySaleItemModel> Detail { get; }
// ReSharper disable UnusedMember.Global
        
        public DailySaleTotal Total
        {
            get
            {
                return new DailySaleTotal
                {
                    Sales = Detail.Sum(f => f.GrossAmount),
                    Profit = Detail.Sum(f => f.GpAmt)
                };
            }
        }
        
    }
    public class DailySaleTotal
    {
        public decimal Sales { get; set; }
        public decimal? Profit { get; set; }
        public decimal? GpPct => Sales == 0 ? null : Profit * 100 / Sales;
    }
}