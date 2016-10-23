using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
	public enum OrderType
	{
		Buy,
		Sell
	}
	public enum OrderStatus
	{
		Active,
		Inactive
	}
    public partial class MarketOrder
    {
        public int ID { get; set; }
        public long EveMarketOrderID { get; set; }
        public int ItemID { get; set; }
        public int EveCharacterID { get; set; }
        public long StationID { get; set; }
        public int OriginalQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public int MinimumQuantity { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Nullable<int> Range { get; set; }
        public int Duration { get; set; }
        public decimal Escrow { get; set; }
        public decimal Price { get; set; }
        public Nullable<OrderType> OrderType { get; set; }
        public System.DateTime WhenIssued { get; set; }
        public Nullable<int> CharacterID { get; set; }
        public Nullable<int> UserID { get; set; }
        public virtual Item Item { get; set; }
        public virtual Station Station { get; set; }
        public virtual User User { get; set; }
    }
}
