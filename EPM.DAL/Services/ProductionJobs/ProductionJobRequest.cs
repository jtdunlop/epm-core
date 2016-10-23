namespace DBSoft.EPM.DAL.Services.ProductionJobs
{
    using CodeFirst.Models;

    public class ProductionJobRequest
    {
        public string Token { get; set; }
        public ProductionJobStatus? Status { get; set; }
    }
}