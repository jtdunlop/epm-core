namespace DBSoft.EPM.DAL.DTOs
{
	public class BuildableItemDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public int MaterialEfficiency { get; set; }
		public int ProductionEfficiency { get; set; }
		public int? MinimumStock { get; set; }
		public int QuantityMultiplier { get; set; }
        public decimal? PerJobAdditionalCost { get; set; }
	    public decimal Volume { get; set; }
	}
}
