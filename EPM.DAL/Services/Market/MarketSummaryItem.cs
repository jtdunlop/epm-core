namespace DBSoft.EPM.DAL.Services.Market
{
    public class MarketSummaryItem
    {
        public decimal? MaximumBuyPrice { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public long SellVolume { get; set; }
        public decimal MinimumSellPrice { get; set; }
        public int Competitors { get; set; }
    }
}