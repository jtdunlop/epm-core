namespace DBSoft.EPM.DAL.Requests
{
	using CodeFirst.Models;

    public class MarketPriceRequest
	{
        public string Token { get; set; }
		public OrderType? OrderType { get; set; }
		public int? Range { get; set; }
		public DateRange DateRange { get; set; }
	}
}