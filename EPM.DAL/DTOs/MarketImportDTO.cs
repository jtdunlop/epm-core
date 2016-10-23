namespace DBSoft.EPM.DAL.DTOs
{
	using System;

	public class MarketImportDTO
	{
		public long MarketImportID { get; set; }
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public DateTime TimeStamp { get; set; }
		public int RegionID { get; set; }
	}
}