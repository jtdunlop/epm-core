namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    using System;

    public partial class ItemExtension
    {
        public int ID { get; set; }
        public int? MinimumStock { get; set; }
        public decimal? BounceFactor { get; set; }
        public int ItemID { get; set; }
        public int? UserID { get; set; }
        public decimal? PerJobAdditionalCost { get; set; }
        public DateTime? LastModified { get; set; }
        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}
