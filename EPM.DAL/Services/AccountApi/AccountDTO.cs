namespace DBSoft.EPM.DAL.Services.AccountApi
{
    using CodeFirst.Models;

    public class AccountDTO
	{
		public int AccountID { get; set; }
		public int ApiKeyID { get; set; }
		public string ApiVerificationCode { get; set; }
		public string AccountName { get; set; }
		public bool DeletedFlag { get; set; }
		public int ApiAccessMask { get; set; }
		public AccountType ApiKeyType { get; set; }
	}
}