namespace DBSoft.EPM.Logic
{
    using System.Threading.Tasks;
    using DAL.Services.AccountApi;

    public interface IEveAccountProcessor
    {
        Task<AccountDTO> SaveAccount(AccountDTO account, string token);
    }
}