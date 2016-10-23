namespace DBSoft.EPM.DAL.Requests
{
	public class SaveAssetRequest
	{
		public string Token { get; set; }
		public long AssetID { get; set; }
		public int ItemID { get; set; }
		public int AccountID { get; set; }
		public int LocationID { get; set; }
		public long Quantity { get; set; }
	}
}