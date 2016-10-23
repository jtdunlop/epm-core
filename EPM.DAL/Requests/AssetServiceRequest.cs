namespace DBSoft.EPM.DAL.Requests
{
	using CodeFirst.Models;

	public class AssetServiceRequest
	{
		public AssetServiceRequest()
		{
			OrderType = OrderType.Buy;
		}
		public string Token { get; set; }
		public int? RegionID { get; set; }
		public int? StationID { get; set; }
		public OrderType OrderType { get; set; }
	    public int? SolarSystemID { get; set; }
	}
}
