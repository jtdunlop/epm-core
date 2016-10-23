using System.Collections.Generic;

namespace DBSoft.EPM.DAL.Services.AccountApi
{
    public interface IAccountApiService
    {
        IEnumerable<AccountApiDTO> List(string token);
    }
}