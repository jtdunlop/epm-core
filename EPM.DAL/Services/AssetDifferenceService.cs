namespace DBSoft.EPM.DAL.Services
{
	using System.Data.Entity;
	using System.Diagnostics.Contracts;
	using CacheAspect.Attributes;
	using DTOs;
	using Interfaces;
	using System.Collections.Generic;
	using System.Linq;
	using Queries;
	using Requests;

	public class AssetDifferenceRequest
	{
		public string Token { get; set; }
		public int SourceLocation { get; set; }
		public int TargetLocation { get; set; }
	}
	
	public class AssetDifferenceService : DataService
	{
		private readonly AssetService _assetsHere;
		private readonly AssetService _assetsThere;
		private readonly DbContext _context;

		public AssetDifferenceService(IDbContextFactory factory) : base(factory)
		{
			_context = factory.CreateContext();
			_assetsHere = new AssetService(factory);
			_assetsThere = new AssetService(factory);
		}

		[Cache.Cacheable]
		public IEnumerable<AssetByItemDTO> List(AssetDifferenceRequest request)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(request.Token));

			var buildable = new BuildableItemQuery(_context, GetUserID(request.Token));
			var here = _assetsHere.ListByItem(new AssetServiceRequest { Token = request.Token, StationID = request.SourceLocation });
			var there = _assetsThere.ListByItem(new AssetServiceRequest { Token = request.Token, StationID = request.TargetLocation });

			var items = from h in here
						join b in buildable.GetQuery().ToList() on h.ItemID equals b.ID
						select new AssetByItemDTO
						{
							ItemID = b.ID,
							ItemName = b.Name
						};

			var result = items
				.Where(f => there.All(g => g.ItemID != f.ItemID))
				.Select(f => new AssetByItemDTO
				{
					ItemID = f.ItemID,
					ItemName = f.ItemName
				});
			return result.ToList();
		}
	}
}
