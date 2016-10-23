using System.Collections.Generic;
using DBSoft.EPM.DAL.Requests;

namespace DBSoft.EPM.DAL.Services.AccountApi
{
    public interface IAccountService
    {
        IEnumerable<AccountDTO> List(AccountRequest request);
        void DeleteAccount(DeleteAccountRequest request);
        void SaveAccount(SaveAccountRequest request);
    }
}