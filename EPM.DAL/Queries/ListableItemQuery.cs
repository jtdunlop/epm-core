namespace DBSoft.EPM.DAL.Queries
{
	using CodeFirst.Models;
	using System.Data.Entity;
	using System.Linq;

	public class ListableItemQuery : DataQuery<Item>
	{
		private IQueryable<BlueprintInstance> _blueprints;

		public ListableItemQuery(DbContext context, int userId)
			: base(context)
		{
			_blueprints = new DataQuery<BlueprintInstance>(context)
				.GetQuery()
				// Consumed BPCs hang around until minimum stock is cleared as they may be replaced by more BPCs
				.Where(f => !f.DeletedFlag && f.Asset.UserID == userId || f.Blueprint.BuildItem.ItemExtensions.FirstOrDefault(g => g.UserID == userId).MinimumStock > 0);
		}

		public override IQueryable<Item> GetQuery()
		{
			return _blueprints
				.Select(f => f.Blueprint.BuildItem)
				.Distinct();
		}

		public ListableItemQuery SpecifyStation(int stationId)
		{
			_blueprints = _blueprints.Where(f => f.Asset.StationID == stationId);
			return this;
		}

        public ListableItemQuery SpecifySolarSystem(int systemId, int stationId)
        {
            _blueprints = _blueprints.Where(f => f.Asset.SolarSystemID == systemId || f.Asset.StationID == stationId);
            return this;
        }
	}
}
