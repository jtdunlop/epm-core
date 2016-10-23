namespace DBSoft.EVEAPI.Plumbing
{
    using System.Threading.Tasks;

    public interface IEveContentLoader
    {
        Task<string> LoadContent(string url);
    }
}