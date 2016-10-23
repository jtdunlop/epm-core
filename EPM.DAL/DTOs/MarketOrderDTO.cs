namespace DBSoft.EPM.DAL.DTOs
{
	public class MarketOrderDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public int RemainingQuantity { get; set; }
		public decimal Price { get; set; }
		public decimal Escrow { get; set; }
	}
}