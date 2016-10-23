namespace DBSoft.EPM.DAL.Services
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using Aspects;
	using CodeFirst.Models;
	using DbSoft.Cache.Aspect.Attributes;
	using DTOs;
	using Interfaces;
	using Queries;
	using Requests;

    public class LocationService : ILocationService
    {
		private readonly DbContext _context;

		public LocationService(IDbContextFactory factory)
		{
			_context = factory.CreateContext();
		}

		[Trace,Cache.Cacheable]
		public IEnumerable<StationDTO> ListStations(StationListRequest request)
		{
			var query = new DataQuery<Station>(_context);
			if (request.RegionID.HasValue)
			{
				query.Specify(f => f.SolarSystem.RegionID == request.RegionID);
			}
			return query.GetQuery().Select(f => new StationDTO
			{
				StationID = f.ID,
				StationName = f.Name,
				SolarSystemID = f.SolarSystemID
			}).ToList();
		}
	}
}
