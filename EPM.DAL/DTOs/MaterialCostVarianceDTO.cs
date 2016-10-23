namespace DBSoft.EPM.DAL.DTOs
{
	public class MaterialCostVarianceDto
	{
		public int MaterialId { get; set; }
		public string MaterialName { get; set; }
		public decimal CostNow { get; set; }
		public decimal CostThen { get; set; }
		public decimal? Variance => CostThen == 0 ? 0 : (CostNow - CostThen) / CostThen * 100;
	}
}
