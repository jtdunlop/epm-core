namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    using System;

    public class MarketResearch
    {
        public int ID { get; set; }
        public long StationID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public TimeSpan ProductionTime { get; set; }
        public long Inventory { get; set; }
        public double Volume { get; set; }
        public int Competitors { get; set; }
        public decimal QuantityMultiplier { get; set; }
    }
}
