namespace DBSoft.EPM.DAL.DTOs
{
    using System;

    public class ItemPriceDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public decimal CurrentPrice { get; set; }
		public decimal Cost { get; set; }
		public decimal? Variance { get; set; }
	    public DateTime Timestamp { get; set; }
	}
}