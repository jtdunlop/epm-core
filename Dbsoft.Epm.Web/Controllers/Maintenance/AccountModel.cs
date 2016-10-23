namespace Dbsoft.Epm.Web.Controllers.Maintenance
{
	public class AccountModel
	{
		// ReSharper disable UnusedMember.Global
		public int AccountID { get; set; }
		public string AccountName { get; set; }
		public int ApiKeyType { get; set; }
		public int ApiKeyID { get; set; }
		public string ApiVerificationCode { get; set; }
		public int ApiAccessMask { get; set; }
		public bool DeletedFlag { get; set; }
		// ReSharper restore UnusedMember.Global
	}
}