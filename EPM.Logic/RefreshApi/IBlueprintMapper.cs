namespace DBSoft.EPM.Logic.RefreshApi
{
    using System.Threading.Tasks;

    public interface IBlueprintMapper
    {
        Task Pull(string token);
    }
}