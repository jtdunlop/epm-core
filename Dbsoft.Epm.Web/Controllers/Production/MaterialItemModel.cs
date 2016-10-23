namespace Dbsoft.Epm.Web.Controllers.Production
{
    using System.ComponentModel;

    public class MaterialItemModel
    {
        public int ItemID { get; set; }
        [DisplayName("Item")]
        public string ItemName { get; set; }
        [DisplayName("Bounce Factor")]
        public decimal? BounceFactor { get; set; }
    }
}