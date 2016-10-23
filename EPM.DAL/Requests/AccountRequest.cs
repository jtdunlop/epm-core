namespace DBSoft.EPM.DAL.Requests
{
	using CodeFirst.Models;

	public class AccountRequest
	{
		public string Token { get; set; }
		public bool IncludeDeleted { get; set; }
		public AccountType? AccountType { get; set; }
	}
}