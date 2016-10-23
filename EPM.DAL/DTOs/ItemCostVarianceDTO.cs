namespace DBSoft.EPM.DAL.DTOs
{
	public class ItemCostVarianceDto
	{
		public int ItemId { get; set; }
		public string ItemName { get; set; }
		public decimal CostNow { private get; set; }
		public decimal CostThen { private get; set; }
		public decimal? Variance
		{
			get
			{
				if ( CostThen != 0 )
					return (CostNow - CostThen) / CostThen * 100;
				return null;
			}
		}
	}
}
