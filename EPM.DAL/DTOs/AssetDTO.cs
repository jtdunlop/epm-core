namespace DBSoft.EPM.DAL.DTOs
{
	public class AssetBaseDTO
	{
		public long Quantity { get; set; }
	}

	public class AssetByItemDTO : AssetBaseDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
	}

	public class AssetByItemAndStationDTO : AssetBaseDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public long? StationID { get; set; }
		public string StationName { get; set; }
	}

}
