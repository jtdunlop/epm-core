namespace DBSoft.EPM.DAL.Queries
{
    using System.Data.Entity;
    using CodeFirst.Models;

    public class BlueprintInstanceQuery : DataQuery<BlueprintInstance>, IBlueprintInstanceQuery
    {
        public BlueprintInstanceQuery(DbContext context, int userId) : base(context)
        {
            Specify(f => !f.Asset.DeletedFlag);
            Specify(f => f.Asset.UserID == userId);
        }

        public IBlueprintInstanceQuery SpecifyStation(int stationId)
        {
            Specify(f => f.Asset.StationID == stationId);
            return this;
        }

        public IBlueprintInstanceQuery SpecifyFactory(int systemId, int stationId)
        {
            return systemId == 0 ? SpecifyStation(stationId) : SpecifySolarSystem(systemId, stationId);
        }

        public IBlueprintInstanceQuery SpecifySolarSystem(int systemId, int stationId)
        {
            Specify(f => f.Asset.SolarSystemID == systemId || f.Asset.StationID == stationId);
            return this;
        }
    }
}