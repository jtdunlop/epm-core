namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public class MarketPrice
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public decimal AdjustedPrice { get; set; }

        public virtual Item Item { get; set; }
    }
}
