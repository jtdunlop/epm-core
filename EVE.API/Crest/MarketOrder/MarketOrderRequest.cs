namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using System.Collections.Generic;
    using Entities.MarketOrder;

    public class MarketOrderRequest
    {
        public MarketOrderRequest()
        {
            ItemIds = new List<int>();
        }

        public string Token { get; set; }
        public List<int> ItemIds { get; set; }
        public OrderType OrderType { get; set; }
        public int RegionId { get; set; }
        public int Range { get; set; }
    }
}