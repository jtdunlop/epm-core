namespace DBSoft.EPM.DAL.Queries
{
	using System.Data.Entity;
	using System.Linq;
	using Models;

	public class AssetItemQuery : DataQuery<Item>
	{
		private IQueryable<Asset> _assets;

		public AssetItemQuery(DbContext context)
			: base(context)
		{
			_assets = new DataQuery<Asset>(context)
				.GetQuery()
				.Where(f => !f.DeletedFlag);
		}

		public override IQueryable<Item> GetQuery()
		{
			return _assets
				.Select(f => f.Item)
				.Distinct();
		}

		public AssetItemQuery SpecifyLocation(int locationId)
		{
			_assets = _assets.Where(f => f.LocationID == locationId);
			return this;
		}
	}
}