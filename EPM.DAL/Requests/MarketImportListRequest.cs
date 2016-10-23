namespace DBSoft.EPM.DAL.Requests
{
	using System;

	public class MarketImportRequest
	{
		public string Token { get; set; }
		public TimeSpan? MaxAge { get; set; }
		public string ItemName { get; set; }
		public int? RegionID { get; set; }
	}
}