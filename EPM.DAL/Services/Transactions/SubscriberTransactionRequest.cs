namespace DBSoft.EPM.DAL.Services.Transactions
{
    using CodeFirst.Models;

    public class SubscriberTransactionRequest
    {
        public string Token { get; set; }
        public DateRange DateRange { get; set; }
        public TransactionType? TransactionType { get; set; }
    }
}