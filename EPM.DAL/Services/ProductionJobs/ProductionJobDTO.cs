namespace DBSoft.EPM.DAL.Services.ProductionJobs
{
    using CodeFirst.Models;

    public class ProductionJobDTO
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public ProductionJobStatus Status { get; set; }
    }
}