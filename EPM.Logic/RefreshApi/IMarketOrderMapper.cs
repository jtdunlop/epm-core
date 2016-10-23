namespace DBSoft.EPM.Logic.RefreshApi
{
    using System.Threading.Tasks;

    public interface IMarketOrderMapper
    {
        Task Pull(string token);
    }
}