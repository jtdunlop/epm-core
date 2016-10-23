namespace DBSoft.EPM.DAL.DTOs
{
	public class ItemCapitalDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public decimal JobValue { get; set; }
		public decimal FactoryValue { get; set; }
		public decimal RemoteValue { get; set; }
		public decimal MarketValue { get; set; }
		public decimal TotalValue
		{
			get
			{
				return JobValue + FactoryValue + RemoteValue + MarketValue;
			}
		}
	}
}