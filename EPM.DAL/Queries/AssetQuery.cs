namespace DBSoft.EPM.DAL.Queries
{
	using System.Data.Entity;
	using CodeFirst.Models;

	public class AssetQuery : DataQuery<Asset>
	{
		public AssetQuery(DbContext context) : base(context)
		{
			Specify(f => !f.DeletedFlag);
		}
	
		public void SpecifyStation(int stationId)
		{
			Specify(f => f.StationID == stationId);
		}

		public void SpecifyRegion(int regionID)
		{
			Specify(f => f.Station.SolarSystem.Region.ID == regionID);
		}

	    public void SpecifySolarSystem(int solarSystemId)
	    {
            Specify(f => f.SolarSystemID == solarSystemId);
	    }
	}
}
