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
    
    public partial class MarketImport
    {
        public long ID { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public int StationID { get; set; }
        public int SolarSystemID { get; set; }
        public int RegionID { get; set; }
        public decimal Price { get; set; }
        public int ItemID { get; set; }
        public short OrderType { get; set; }
        public short Jumps { get; set; }
        public short Range { get; set; }
    
        public virtual Item Item { get; set; }
    }
}
