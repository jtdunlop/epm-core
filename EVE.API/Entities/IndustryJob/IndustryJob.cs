namespace DBSoft.EVEAPI.Entities.IndustryJob
{
	using System;

	public enum IndustryJobStatus
	{
		InProgress,
		Delivered,
	}
	public class IndustryJob
	{
		public int ID { get; set; }
		public long InstalledItemID { get; set; }
		public int LicensedRuns { get; set; }
		public int InstallerID { get; set; }
		public int Runs { get; set; }
		public int InstalledItemTypeID { get; set; }
		public int OutputTypeID { get; set; }
		public bool Completed { get; set; }
		public int ActivityID { get; set; }
		public DateTime? BeginProductionTime { get; set; }
		public DateTime? EndProductionTime { get; set; }
		public DateTime? PauseProductionTime { get; set; }
	}
}
