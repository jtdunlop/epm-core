namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using System.Collections.Generic;
    using Entities.MarketOrder;

    public class Order
    {
        public long OrderID { get; set; }
        public long StationID { get; set; }
        public string StationName { get; set; }
        public double Price { get; set; }
        public int ItemID { get; set; }
        public OrderType OrderType { get; set; }
        public short Range { get; set; }
    }

    public class MarketOrderDTO
    {
        public MarketOrderDTO()
        {
            Orders = new List<Order>();
            Citadels = new List<Citadel>();
        }
        public List<Order> Orders { get; set; } 
        public List<Citadel> Citadels { get; set; } 
    }

    public class Citadel
    {
        public string Name { get; set; }
        public long Id { get; set; }
    }
}