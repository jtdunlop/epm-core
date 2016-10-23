namespace DBSoft.EPM.DAL.Services
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using CacheAspect.Attributes;
	using Interfaces;
	using Models;
	using Queries;

	public class StationListRequest
	{
		public int? RegionID { get; set; }
	}
	
	public class StationDTO
	{
		public int StationID { get; set; }
		public string StationName { get; set; }
	}

	public class StationService
	{
		private readonly DbContext _context;

		public StationService(IDbContextFactory factory)
		{
			_context = factory.CreateContext();
		}

		[Cache.Cacheable]
		public IEnumerable<StationDTO> List(StationListRequest request)
		{
			var query = new DataQuery<Station>(_context);
			if (request.RegionID.HasValue)
			{
				query.Specify(f => f.SolarSystem.RegionID == request.RegionID);
			}
			return query.GetQuery().Select(f => new StationDTO
			{
				StationID = f.ID,
				StationName = f.Name
			}).ToList();
		}
	}
}
