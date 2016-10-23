namespace DBSoft.EPM.DAL.Queries
{
	using System.Data.Entity;
	using System.Linq;
	using Models;

	public class MarketItemQuery : DataQuery<Item>
	{
		private readonly IQueryable<MarketOrder> _orders;

		public MarketItemQuery(DbContext context)
			: base(context)
		{
			_orders = new MarketOrderQuery(context)
				.GetQuery();
		}

		public override IQueryable<Item> GetQuery()
		{
			return _orders.Where(f => f.RemainingQuantity > 0).Select(f => f.Item);
		}
	}
}
