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
    
    public partial class Station
    {
        public Station()
        {
            this.MarketOrders = new HashSet<MarketOrder>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int SolarSystemID { get; set; }
    
        public virtual ICollection<MarketOrder> MarketOrders { get; set; }
        public virtual SolarSystem SolarSystem { get; set; }
    }
}
