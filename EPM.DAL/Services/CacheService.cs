namespace DBSoft.EPM.DAL.Services
{
    using System.Linq;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Transactions;

    public class DailySalesService
    {
        private readonly IItemTransactionService _sales;

        public DailySalesService(IItemTransactionService sales)
        {
            _sales = sales;
        }

        [Cache.Cacheable(ttl: 3600 * 24)]
        public IOrderedEnumerable<ItemTransactionByDateDto> ListDailySales(DateRange range, string tenantId)
        {
            var result = _sales
                .ListByDate(new ItemTransactionRequest
                {
                    TenantId = tenantId,
                    DateRange = range,
                    TransactionType = TransactionType.Sell
                })
                .OrderByDescending(o => o.DateTime);

            return result;
        }

    }
}

