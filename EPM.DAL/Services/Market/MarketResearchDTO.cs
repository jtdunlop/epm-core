namespace DBSoft.EPM.DAL.Services.Market
{
    using System;

    public class MarketResearchDTO
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public TimeSpan ProductionTime { get; set; }
        public long Inventory { get; set; }
        public double Volume { get; set; }
        public decimal IskPerHour { get { return (Price - Cost) / (decimal) ProductionTime.TotalHours * QuantityMultiplier; } }
        public decimal QuantityMultiplier { get; set; }
        public bool IsMine { get; set; }

        public decimal ProfitFactor
        {
            get
            {
                return Competitors == 0 ? 0 : Math.Min((Price - Cost) * (decimal)Volume / Competitors, IskPerHour * 23 / Competitors);
            }
        }

        public int Competitors { get; set; }
    }
}