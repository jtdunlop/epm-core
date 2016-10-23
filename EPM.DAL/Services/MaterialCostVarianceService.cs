namespace DBSoft.EPM.DAL.Services
{
	using CacheAspect.Attributes;
	using DTOs;
	using Interfaces;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Requests;

	public class MaterialCostVarianceService
	{
		private readonly MaterialCostService _costNow;
		private readonly MaterialCostService _costThen;

		public MaterialCostVarianceService(IDbContextFactory factory)
		{
			_costNow = new MaterialCostService(factory);
			_costThen = new MaterialCostService(factory);
		}

		[Cache.Cacheable]
		public IEnumerable<MaterialCostVarianceDTO> List(string token)
		{
			var costNow = _costNow
				.List(new MaterialCostRequest { Token = token, Date = DateTime.Now });
			var costThen = _costThen
				.List(new MaterialCostRequest { Token = token, Date = DateTime.Now.AddDays(-7) });

			var result = from now in costNow
					join then in costThen on now.ItemID equals then.ItemID
					select new MaterialCostVarianceDTO
					{
						ItemID = now.ItemID,
						ItemName = now.ItemName,
						CostNow = now.Price,
						CostThen = then.Price
					};

			return result.ToList();
		}
	}
}
