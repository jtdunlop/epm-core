namespace DBSoft.EPM.DAL.DTOs
{
    using System;

    public class MarketPriceDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public decimal CurrentPrice { get; set; }
	    public DateTime Timestamp { get; set; }
	}
}
