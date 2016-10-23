namespace DBSoft.EPM.DAL.Services.MaterialCosts
{
	public class MaterialCostDTO
	{
		public int MaterialID { get; set; }
		public string MaterialName { get; set; }
		public decimal? HubPrice { get; set; }
		public decimal? HistoryPrice { get; set; }
        public decimal FreightCost { get; set; }
        public decimal BaseCost { get { return HistoryPrice ?? HubPrice ?? 0; } }
		public decimal Cost
		{
			get
			{
				return FreightCost + BaseCost;
			}
		}
	}
}
