namespace DBSoft.EPM.DAL.DTOs
{
    public class ProductionMaterialDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public long Inventory { get; set; }
        public long Required { get; set; }
        public decimal BounceFactor { get; set; }
    }
}
