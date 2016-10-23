namespace DBSoft.EPM.DAL.Services
{
	using CacheAspect.Attributes;
	using DTOs;
	using Interfaces;
	using Models;
	using Queries;
	using System.Collections.Generic;
	using System.Linq;

	public class BuildableItemService : DataService
	{
		private readonly DataQuery<BlueprintInstance> _blueprints;
		private readonly DataQuery<Item> _items;

		public BuildableItemService(IDbContextFactory factory) : base(factory)
		{
			var context = factory.CreateContext();
			_blueprints = new DataQuery<BlueprintInstance>(context).Specify(f => !f.DeletedFlag);
			_items = new DataQuery<Item>(context);
		}

		[Cache.Cacheable]
		public IEnumerable<BuildableItemDTO> List(string token)
		{
			
			var items = _items.GetQuery();
			var userID = GetUserID(token);
			var blueprints = _blueprints.Specify(f => f.Asset.UserID == userID).GetQuery();

			var result = from item in items
						 join blueprint in blueprints on item.ID equals blueprint.Blueprint.BuildItemID
						 select new BuildableItemDTO
						 {
							 ItemID = item.ID,
							 ItemName = item.Name,
							 MaterialEfficiency = blueprint.MaterialEfficiency,
							 ProductionEfficiency = blueprint.ProductionEfficiency,
							 MinimumStock = item.ItemExtensions.FirstOrDefault(f => f.UserID == userID).MinimumStock
						 };
			return result.ToList();
		}
	}
}
