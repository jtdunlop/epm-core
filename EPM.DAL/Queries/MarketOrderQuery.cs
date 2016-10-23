namespace DBSoft.EPM.DAL.Queries
{
	using System.Data.Entity;
	using CodeFirst.Models;

	public class MarketOrderQuery : DataQuery<MarketOrder>
	{
		public MarketOrderQuery(DbContext context) : base(context)
		{
			Specify(f => f.RemainingQuantity > 0 && 
				f.OrderStatus == 0);
		}
	}
}
