namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using System.Collections.Generic;

    public class MarketOrderResponse
    {
        public string TotalCountStr { get; set; }
        public List<Item> Items { get; set; }
        public int PageCount { get; set; }
        public string PageCountStr { get; set; }
        public int TotalCount { get; set; }
    }

    public class MarketOrderItem
    {
        public bool Buy { get; set; }
        public string Issued { get; set; }
        public double Price { get; set; }
        public long Volume { get; set; }
        public int Duration { get; set; }
        public long ID { get; set; }
        public int MinVolume { get; set; }
        public int VolumeEntered { get; set; }
        public string Range { get; set; }
        public long StationID { get; set; }
        public int Type { get; set; }
    }

    public class Next
    {
        public string Href { get; set; }
    }

    public class BulkMarketOrderResponse
    {
        public List<MarketOrderItem> Items { get; set; }
        public int TotalCount { get; set; }
        public Next Next { get; set; }
        public int PageCount { get; set; }
    }
}