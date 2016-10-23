namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Requests;
    using Services.ProductionJobs;

    public interface IProductionJobService
	{
		void SaveProductionJobs(IEnumerable<SaveProductionJobRequest> request);
		IEnumerable<ProductionJobDTO> List(ProductionJobRequest request);
        Task DeactivateJobs(string token);
	}
}