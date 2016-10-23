namespace DBSoft.EPM.Logic.RefreshApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DAL.Annotations;
    using DAL.CodeFirst.Models;
    using DAL.Interfaces;
    using DAL.Requests;
    using DAL.Services.AccountApi;
    using EVEAPI.Entities.IndustryJob;

    public interface IIndustryJobMapper
    {
        Task Pull(string token);
    }

    [UsedImplicitly]
    public class IndustryJobMapper : EveApiMapper, IIndustryJobMapper
    {

		private readonly IProductionJobService _productionJobService;
	    private readonly IAccountApiService _accounts;
        private readonly IIndustryJobService _jobs;

        public IndustryJobMapper(IProductionJobService productionJobService, 
            IEveApiStatusService statusService, IAccountApiService accounts, IIndustryJobService jobs) : base(statusService)
		{
            _productionJobService = productionJobService;
		    _accounts = accounts;
            _jobs = jobs;
		}

		public async Task Pull(string token)
		{
			const string serviceName = "IndustryJobs";
            
			var cachedUntil = DateTime.Now;
			await _productionJobService.DeactivateJobs(token);
            foreach (var account in _accounts.List(token))
            {
                try
                {
                    var result = await _jobs.Load(account.ApiKeyType, account.ApiKeyID, account.ApiVerificationCode, account.EveApiID);
                    ProcessJobs(result.Data, token);
                    cachedUntil = result.CachedUntil;
                }
                catch (Exception e)
                {
                    SaveError(serviceName, token, e.Message);
                    throw;
                }
            }
			UpdateStatus(serviceName, cachedUntil, token);
		}

		private void ProcessJobs(IEnumerable<IndustryJob> eveJobs, string token)
		{
            var requests = eveJobs.Select(job => new SaveProductionJobRequest
            {
                Token = token,
                ProductionJobID = job.ID,
                ItemID = job.OutputTypeID,
                AssetID = job.InstalledItemID,
                Quantity = job.Runs,
                Status = GetJobStatus(job.Completed),
                WhenCompleted = job.EndProductionTime,
                WhenInstalled = job.BeginProductionTime.GetValueOrDefault(),
                WhenPaused = job.PauseProductionTime,
                WhenStarted = job.BeginProductionTime
            }).ToList();
		    _productionJobService.SaveProductionJobs(requests);
		}

		private static ProductionJobStatus GetJobStatus(bool completed)
		{
			return completed ? ProductionJobStatus.Complete : ProductionJobStatus.Active;
		}
	}
}
