namespace DBSoft.EPM.DAL.Queries
{
	using System.Data.Entity;
	using System.Linq;
	using CodeFirst.Models;

	public class ProductionItemQuery : DataQuery<Item>
	{
		private readonly DataQuery<ProductionJob> _jobs;

		public ProductionItemQuery(DbContext context)
			: base(context)
		{
			_jobs = new DataQuery<ProductionJob>(context);
		}

		public ProductionItemQuery SpecifyStatus(ProductionJobStatus status)
		{
			_jobs.Specify(f => f.Status == status);
			return this;
		}

		public ProductionItemQuery SpecifyUser(int userID)
		{
			_jobs.Specify(f => f.Asset.UserID == userID);
			return this;
		}

		public override IQueryable<Item> GetQuery()
		{
			return _jobs
				.GetQuery()
				.Select(f => f.Item)
				.Distinct();
		}
	}
}
