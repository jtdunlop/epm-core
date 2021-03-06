//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPM.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class MarketOrder
    {
        public long ID { get; set; }
        public int ItemID { get; set; }
        public int CharacterID { get; set; }
        public int StationID { get; set; }
        public int OriginalQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public int MinimumQuantity { get; set; }
        public int OrderStatus { get; set; }
        public Nullable<int> Range { get; set; }
        public int Duration { get; set; }
        public decimal Escrow { get; set; }
        public decimal Price { get; set; }
        public int OrderType { get; set; }
        public System.DateTime WhenIssued { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Station Station { get; set; }
    }
}
