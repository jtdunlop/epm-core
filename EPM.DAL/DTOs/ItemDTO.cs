namespace DBSoft.EPM.DAL.DTOs
{
	public class ItemDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
        public int QuantityMultiplier { get; set; }
        public decimal Volume { get; set; }
	}
}
