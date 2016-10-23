namespace DBSoft.EPM.DAL.DTOs
{
    using System;

    public class MaterialRequirementDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public long Quantity => (long) Math.Ceiling((1 - .01*MaterialEfficiency)*(BaseQuantity + AdditionalQuantity));
        public long BaseQuantity { get; set; }
        public long AdditionalQuantity { get; set; }
        public int MaterialEfficiency { get; set; }
        public decimal? BounceFactor { get; set; }
    }
}
