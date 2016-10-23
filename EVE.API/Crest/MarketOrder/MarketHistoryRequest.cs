namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using System.Collections.Generic;

    public class MarketHistoryRequest
    {
        public int RegionID { get; set; }
        public List<int> ItemIds { get; set; }
    }
}