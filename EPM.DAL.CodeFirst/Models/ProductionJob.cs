using System;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
	public enum ProductionJobStatus
	{
		Active,
		Complete
	}
    public partial class ProductionJob
    {
        public long ID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public ProductionJobStatus Status { get; set; }
        public DateTime WhenInstalled { get; set; }
        public DateTime? WhenStarted { get; set; }
        public DateTime? WhenCompleted { get; set; }
        public DateTime? WhenPaused { get; set; }
        public long? AssetID { get; set; }
        public decimal TeamSavings { get; set; }
        public int? HiredTeamID { get; set; }
        public int? UserID { get; set; }
        public virtual Asset Asset { get; set; }
        public virtual Item Item { get; set; }
        public virtual HiredTeam HiredTeam { get; set; }
        public virtual User User { get; set; }
    }
}
