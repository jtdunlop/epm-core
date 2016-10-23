namespace DBSoft.EPM.DAL.Requests
{
	using CodeFirst.Models;

	public class SaveMarketImportRequest
	{
		public int ItemID { get; set; }
		public short Jumps { get; set; }
		public short Range { get; set; }
		public OrderType OrderType { get; set; }
		public long StationID { get; set; }
		public int RegionID { get; set; }
		public int SolarSystemID { get; set; }
		public decimal Price { get; set; }
	}
}