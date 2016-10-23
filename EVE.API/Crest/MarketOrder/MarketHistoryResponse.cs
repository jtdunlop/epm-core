namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using System.Collections.Generic;

    public class MarketHistoryResponse
    {
        public string totalCount_str { get; set; }
        public List<HistoryItem> items { get; set; }
        public int pageCount { get; set; }
        public string pageCount_str { get; set; }
        public int totalCount { get; set; }
    }
}