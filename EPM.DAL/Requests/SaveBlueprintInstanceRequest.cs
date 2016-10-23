namespace DBSoft.EPM.DAL.Requests
{
	public class SaveBlueprintInstanceRequest
	{
		public string Token { get; set; }
		public long AssetID { get; set; }
		public long BlueprintItemID { get; set; }
		public int? MaterialEfficiency { get; set; }
		public int? ProductionEfficiency { get; set; }
		public bool IsCopy { get; set; }
	}
}