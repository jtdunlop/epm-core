using System;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using EPM.DAL.Queries;

namespace EPM.DAL.Services
{
	public class BaseSaleService
	{
		private readonly DataQuery<Item> _itemQuery;
		private readonly DataQuery<Transaction> _transactionQuery;

		protected BaseSaleService(DbContext context)
		{
			_itemQuery = new DataQuery<Item>(context);
			_transactionQuery = new DataQuery<Transaction>(context);
			_transactionQuery.Specify(f => f.TransactionType == TransactionType.Sell);
		}

		protected IQueryable<ItemTransaction> GetQuery(DateTime startDate, DateTime endDate)
		{
			_transactionQuery.Specify(f => f.DateTime >= startDate && f.DateTime <= endDate);
			var joined = _transactionQuery.GetQuery().Join(_itemQuery.GetQuery(), outer => outer.ItemID, inner => inner.ID, (outer, inner) => new ItemTransaction
			{
				ItemID = inner.ID,
				ItemName = inner.Name,
				Quantity = outer.Quantity,
				Price = outer.Price,
				Cost = outer.Cost,
				TransactionDate = outer.DateTime
			});
			return joined;
		}

		protected void SpecifyDateRange(DateTime fromDate, DateTime toDate)
		{
			_transactionQuery.Specify(f => f.DateTime >= fromDate && f.DateTime <= toDate);
		}

		protected class ItemTransaction
		{
			public int ItemID { get; set; }
			public string ItemName { get; set; }
			public DateTime TransactionDate { get; set; }
			public decimal Quantity { get; set; }
			public decimal Price { get; set; }
			public decimal? Cost { get; set; }
		}
	}
}