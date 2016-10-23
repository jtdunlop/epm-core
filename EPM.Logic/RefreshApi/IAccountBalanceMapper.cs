namespace DBSoft.EPM.Logic.RefreshApi
{
    using System.Threading.Tasks;

    public interface IAccountBalanceMapper
    {
        Task Pull(string tk);
    }
}