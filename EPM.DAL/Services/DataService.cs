namespace DBSoft.EPM.DAL.Services
{
	public class DataService
	{
	    private readonly IUserService _users;

	    protected DataService(IUserService users)
	    {
	        _users = users;
	    }

	    protected int GetUserID(string token)
		{
			return _users.GetAuthenticatedUser(token).UserID;
		}
	}
}