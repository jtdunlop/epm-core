namespace DBSoft.EPM.DAL.Requests
{
	using CodeFirst.Models;

	public class MarketRepriceRequest
	{
		public string Token { get; set; } 
		public OrderType? OrderType { get; set; }
	}
}