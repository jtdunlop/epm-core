namespace DBSoft.EVEAPI.Crest
{
    using System.Threading.Tasks;

    public interface IUserAuth
    {
        Task<AuthenticatedUserDTO> GetAuthenticatedUser(string code, string type, string clientId, string secret);
        Task<AuthenticatedUserDTO> RefreshAuthenticatedUser(string refresh, string clientId, string secret);
    }
}