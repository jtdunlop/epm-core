namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using System.Collections.Generic;
    using Entities.MarketOrder;

    public class MarketSummaryRequest
    {
        public int StationID { get; set; }
        public List<int> ItemIDs { get; set; }
        public OrderType OrderType { get; set; }
        public int RegionId { get; set; }
    }
}