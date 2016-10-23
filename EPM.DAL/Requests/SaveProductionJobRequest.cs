namespace DBSoft.EPM.DAL.Requests
{
	using System;
	using CodeFirst.Models;

	public class SaveProductionJobRequest
	{
		public string Token { get; set; }
		public long ProductionJobID { get; set; }
		public int ItemID { get; set; }
		public long AssetID { get; set; }
		public int Quantity { get; set; }
		public ProductionJobStatus Status { get; set; }
		public DateTime WhenInstalled { get; set; }
		public DateTime? WhenStarted { get; set; }
		public DateTime? WhenPaused { get; set; }
		public DateTime? WhenCompleted { get; set; }
	}
}