namespace DBSoft.EPM.DAL.Requests
{
	public class AccountBalanceUpdateRequestItem
	{
		public int AccountID { get; set; }
		public int AccountKey { get; set; }
		public decimal Balance { get; set; }
	}
}