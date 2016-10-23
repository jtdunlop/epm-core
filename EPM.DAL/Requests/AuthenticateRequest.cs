namespace DBSoft.EPM.DAL.Requests
{
	public class AuthenticateRequest
	{
		public string Login { get; set; }
		public string Password { get; set; }
		public string IpAddress { get; set; }
	}
}