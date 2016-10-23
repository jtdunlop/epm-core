namespace DBSoft.EPM.DAL.Services.Transactions
{
    using CodeFirst.Models;

    public class ItemTransactionRequest
	{
		public string Token { get; set; }
        public string TenantId { get; set; }
		public DateRange DateRange { get; set; }
		public TransactionType? TransactionType { get; set; }
		public int? ItemID { get; set; }
	}
}
