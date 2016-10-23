namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public class Manifest
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int MaterialItemID { get; set; }
        public int Quantity { get; set; }
        public int? AdditionalQuantity { get; set; }
        public virtual Item Item { get; set; }
        public virtual Item MaterialItem { get; set; }
    }
}
