using System.Threading.Tasks;

namespace DBSoft.EPM.DAL.Services
{
    using System.Collections.Generic;
    using DTOs;
    using Requests;

    public interface IUserService
    {
        AuthenticateResponse Authenticate(SsoAuthenticateRequest request);
        void Impersonate(string impersonate, string token);
        List<UserDTO> List();
        Task<UserDTO> GetUser(string name);
        Task<UserDTO> GetUser(int id);
        int GetUserID(string token);
        void Logout(string token);
        UserDTO GetAuthenticatedUser(string token);
        bool IsAdmin(string token);
    }
}