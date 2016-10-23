namespace DBSoft.EPM.DAL.Services.ProductionJobs
{
    using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
    using System.Threading.Tasks;
    using Annotations;
    using Aspects;
    using AutoMapper;
	using CodeFirst.Models;
    using Commands;
    using DbSoft.Cache.Aspect.Attributes;
    using Interfaces;
	using Queries;
	using Requests;

    [UsedImplicitly]
    public class ProductionJobService : IProductionJobService
	{
        private readonly IUserService _users;
        private readonly DbContext _context;

	    public ProductionJobService(IDbContextFactory factory, IUserService users)
		{
            _context = factory.CreateContext();
	        _users = users;
		}

		public void SaveProductionJobs(IEnumerable<SaveProductionJobRequest> requests)
		{
			using (var cmd = new DataCommand(_context))
			{
                var b = _context.Configuration.AutoDetectChangesEnabled;
                _context.Configuration.AutoDetectChangesEnabled = false;
                foreach (var request in requests)
                {
                    SaveJob(request, cmd);
                }
                _context.ChangeTracker.DetectChanges();
                _context.Configuration.AutoDetectChangesEnabled = b;
                _context.SaveChanges();
            }
		}

	    private void SaveJob(SaveProductionJobRequest request, DataCommand cmd)
	    {
            var userID = _users.GetUserID(request.Token);
	        var job = cmd.Get<ProductionJob>(f => f.ID == request.ProductionJobID);
            if ( AreEquivalent(job, request, userID) )
            {
                return;
            }
	        Mapper.Map(request, job);
            job.UserID = userID;
            var asset = _context.Set<Asset>().SingleOrDefault(f => f.ID == job.AssetID);
            if (job.ItemID != 0 && asset != null) return;
	        if ( asset != null )
	        {
                if ( job.ItemID == 0 && asset.BlueprintInstances.Any() )
                {
                    job.ItemID = asset.BlueprintInstances.First().Blueprint.BuildItemID.GetValueOrDefault();
                }
	        }
	        else
	        {
	            job.AssetID = null;
	        }
	    }

        private static bool AreEquivalent(ProductionJob job, SaveProductionJobRequest request, int userID)
        {
            return job.UserID == userID &&
                job.ItemID == request.ItemID &&
                job.Status == request.Status && 
                job.WhenCompleted == request.WhenCompleted;
        }

        [Trace,Cache.Cacheable]
        public IEnumerable<ProductionJobDTO> List(ProductionJobRequest request)
		{
			var userID = _users.GetUserID(request.Token);
			var query = new DataQuery<ProductionJob>(_context)
				.Specify(f => f.Asset.UserID == userID);
			if ( request.Status.HasValue)
			{
				query.Specify(f => f.Status == request.Status.Value);
			}
			return query.GetQuery().Select(f => new ProductionJobDTO
			{
				ItemID = f.ItemID,
				ItemName = f.Item.Name,
				Status = f.Status,
				Quantity = f.Quantity
			}).ToList();
		}

		public async Task DeactivateJobs(string token)
		{
			var userID = _users.GetUserID(token);
			var jobs = _context.Set<ProductionJob>().Where(f => f.Status == ProductionJobStatus.Active && f.Asset.UserID == userID);
			foreach (var job in jobs)
			{
				job.Status = ProductionJobStatus.Complete;
			}
			await _context.SaveChangesAsync();
		}
	}
}
