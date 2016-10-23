namespace DBSoft.EPM.DAL.Services
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using CacheAspect.Attributes;
	using DTOs;
	using Interfaces;
	using Models;
	using Queries;

	public class RegionService
	{
		private readonly DbContext _context;

		public RegionService(IDbContextFactory factory)
		{
			_context = factory.CreateContext();
		}

		[Cache.Cacheable]
		public IEnumerable<RegionDTO> List()
		{
			return new DataQuery<Region>(_context).GetQuery().Select(f => new RegionDTO
				{
					RegionID = f.ID,
					RegionName = f.Name
				}).ToList();
		}
	}
}
