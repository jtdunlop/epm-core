namespace DBSoft.EPM.DAL.DTOs
{
	public class MaterialCapitalDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public decimal FactoryValue { get; set; }
		public decimal RemoteValue { get; set; }
		public decimal MarketValue { get; set; }
	}
}