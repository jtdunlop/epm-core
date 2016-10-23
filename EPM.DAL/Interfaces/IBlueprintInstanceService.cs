namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Requests;
    using Services;

    public interface IBlueprintInstanceService
    {
        void SaveBlueprintInstance(SaveBlueprintInstanceRequest request);
        Task DeleteAll(string token);
        List<BlueprintInstanceDto> ListBuildable(string token);
        List<BlueprintInstanceDto> ListDistinctBuildable(string token);
        List<BlueprintInstanceDto> ListDistinctOwned(string token);
    }
}