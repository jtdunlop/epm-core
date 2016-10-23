namespace DBSoft.EPM.DAL.Requests
{
	public class AssetDifferenceRequest
	{
		public string Token { get; set; }
		public int SourceLocation { get; set; }
		public int TargetLocation { get; set; }
	}
}