namespace DBSoft.EPM.DAL.Requests
{
	public class AuthenticateResponse
	{
		public string AuthenticationToken { get; set; }
		public string ErrorMessage { get; set; }
	}
}