namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using Entities.MarketOrder;

    public class MarketSummaryDTO
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public long SellVolume { get; set; }
        public decimal MinimumSellPrice { get; set; }
        public decimal MaximumBuyPrice { get; set; }
        public string Exception { get; set; }
        public int Competitors { get; set; }
        public int StationID { get; set; }
        public OrderType OrderType { get; set; }
        public short Range { get; set; }
        public int Price { get; set; }
    }
}