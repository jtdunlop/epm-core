namespace DBSoft.EPM.Logic.RefreshApi
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAssetMapper
    {
        Task Pull(string tk);
    }
}