namespace DBSoft.EPM.DAL.DTOs
{
	public class AssetValueByStationDTO : AssetValueDTO
	{
		public long? StationID { get; set; }
		public string StationName { get; set; }
	}

	public class AssetValueByStationAndItemDTO : AssetValueByStationDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
	}

	public class AssetValueDTO
	{
		public decimal Value { get; set; }
	}
}
