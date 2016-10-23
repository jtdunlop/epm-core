namespace DBSoft.EVEAPI.Plumbing
{
    using System.Threading.Tasks;

    public interface IEveApiLoader
    {
        Task<EveApiLoaderResponse> Load(string url);
    }
}