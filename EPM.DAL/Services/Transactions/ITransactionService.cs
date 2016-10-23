namespace DBSoft.EPM.DAL.Services.Transactions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITransactionService
    {
        long GetLastTransactionID(string token);
        Task SaveTransactions(string token, IEnumerable<SaveTransactionRequest> requests);
    }
}