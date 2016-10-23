namespace DBSoft.EPM.DAL.Requests
{
	public class RegisterUserResponse
	{
		public RegisterUserResponseType RegisterAccountResultType { get; set; }
	}

	public enum RegisterUserResponseType
	{
		Success,
		DuplicateLogin
	}
}