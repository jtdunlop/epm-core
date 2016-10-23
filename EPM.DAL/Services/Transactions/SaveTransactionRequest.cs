namespace DBSoft.EPM.DAL.Services.Transactions
{
    using System;
    using CodeFirst.Models;

    public class SaveTransactionRequest
	{
		public long EveTransactionID { get; set; }
		public int ItemID { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public TransactionType TransactionType { get; set; }
		public DateTime DateTime { get; set; }
		public decimal? Cost { get; set; }
		public bool VisibleFlag { get; set; }
	}
}