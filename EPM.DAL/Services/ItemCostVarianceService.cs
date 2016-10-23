namespace DBSoft.EPM.DAL.Services
{
	using CacheAspect.Attributes;
	using DTOs;
	using Interfaces;
	using Queries;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using Requests;

	public class ItemCostVarianceService : DataService
	{
		private readonly DbContext _context;
		private readonly IDbContextFactory _factory;

		public ItemCostVarianceService(IDbContextFactory factory) : base(factory)
		{
			_factory = factory;
			_context = _factory.CreateContext();
		}

		[Cache.Cacheable]
		public IEnumerable<ItemCostVarianceDTO> List(string token)
		{
			var buildable = new BuildableItemQuery(_context, GetUserID(token)).GetQuery().ToList();
			var costNow = new ItemCostService(_factory).List(new ItemCostRequest 
			{ 
				Token = token,
				Date = DateTime.Now.StartOfTheDay() 
			});
			var costThen = new ItemCostService(_factory).List(new ItemCostRequest 
			{ 
				Token = token,
				Date = DateTime.Now.AddDays(-7).StartOfTheDay() 
			});

			var result = from b in buildable
						 join now in costNow on b.ID equals now.ItemID
						 join then in costThen on b.ID equals then.ItemID
						 select new ItemCostVarianceDTO
						 {
							 ItemID = b.ID,
							 ItemName = b.Name,
							 CostNow = now.Cost,
							 CostThen = then.Cost
						 };
			return result;
		}

	}


}
