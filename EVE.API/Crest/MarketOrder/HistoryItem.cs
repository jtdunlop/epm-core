namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using System;

    public class HistoryItem
    {
        public string volume_str { get; set; }
        public int orderCount { get; set; }
        public double lowPrice { get; set; }
        public double highPrice { get; set; }
        public double avgPrice { get; set; }
        public double volume { get; set; }
        public string orderCount_str { get; set; }
        public DateTime date { get; set; }
    }
}