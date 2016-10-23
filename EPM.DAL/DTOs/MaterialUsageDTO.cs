namespace DBSoft.EPM.DAL.DTOs
{
	public class MaterialUsageDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public int MaterialID { get; set; }
		public string MaterialName { get; set; }
		public long Quantity
		{
			get
			{
				return (long)((1 + .1 / (MaterialEfficiency + 1)) * BaseQuantity + AdditionalQuantity);
			}
		}
		public long BaseQuantity { get; set; }
		public long AdditionalQuantity { get; set; }
		public int MaterialEfficiency { get; set; }
		public decimal? BounceFactor { get; set; }
	}
}
