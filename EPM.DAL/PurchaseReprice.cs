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
    
    public partial class PurchaseReprice
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> RangePrice { get; set; }
        public decimal MyPrice { get; set; }
        public Nullable<decimal> AmarrPrice { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public Nullable<decimal> CurrentPrice { get; set; }
        public Nullable<long> QuantityAvailable { get; set; }
        public Nullable<long> QuantityRequired { get; set; }
        public Nullable<int> ProductionQuantityNeeded { get; set; }
        public Nullable<long> ProductionQuantityAvailable { get; set; }
    }
}
