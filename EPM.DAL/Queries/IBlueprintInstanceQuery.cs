namespace DBSoft.EPM.DAL.Queries
{
    using CodeFirst.Models;

    public interface IBlueprintInstanceQuery : IDataQuery<BlueprintInstance>
    {
        IBlueprintInstanceQuery SpecifyStation(int stationId);
        IBlueprintInstanceQuery SpecifySolarSystem(int systemId, int stationId);
        IBlueprintInstanceQuery SpecifyFactory(int systemId, int stationId);
    }
}