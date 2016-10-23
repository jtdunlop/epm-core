namespace DBSoft.EPM.DAL.DTOs
{
	public class BuildMaterialDto
	{
		public int ItemId { get; set; }
		public string ItemName { get; set; }
		public int MaterialId { get; set; }
		public string MaterialName { get; set; }
		public long Quantity { get; set; }
		public long AdditionalQuantity { get; set; }
		public decimal? BounceFactor { get; set; }
		public int QuantityMultiplier { get; set; }
        public decimal Volume { get; set; }
	}
}
