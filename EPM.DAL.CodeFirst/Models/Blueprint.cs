namespace DBSoft.EPM.DAL.CodeFirst.Models
{
	using System.Collections.Generic;

	public class Blueprint
	{
		public Blueprint()
		{
			BlueprintInstances = new List<BlueprintInstance>();
		}

		public long ID { get; set; }
		public int ItemID { get; set; }
		public int ProductionTime { get; set; }
		public int? BuildItemID { get; set; }

        public virtual Item BuildItem { get; set; }
		public virtual Item Item { get; set; }
		public ICollection<BlueprintInstance> BlueprintInstances { get; set; }
	}
}
