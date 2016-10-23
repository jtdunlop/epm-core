namespace DBSoft.EVEAPI.Entities.MarketPrice
{
    using System.Collections.Generic;

    public class RootObject
    {
        public string totalCount_str { get; set; }
        public List<Item> items { get; set; }
        public int pageCount { get; set; }
        public string pageCount_str { get; set; }
        public int totalCount { get; set; }
    }
}