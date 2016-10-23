namespace DBSoft.EPM.DAL.Requests
{
	public class UpdateItemRequest
	{
		public string Token { get; set; }
		public int ItemID { get; set; }
		public int? MinimumStock { get; set; }
        public decimal? PerJobAdditionalCost { get; set; }
	}
}