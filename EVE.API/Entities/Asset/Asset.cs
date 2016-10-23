namespace DBSoft.EVEAPI.Entities.Asset
{
	public class Asset
	{
		public long ID { get; set; }
		public int ItemID { get; set; }
		public int Quantity { get; set; }
		public int? LocationID { get; set; }
		public bool IsPackaged { get; set; }
	}
}