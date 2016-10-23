using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Station
    {
        public Station()
        {
            this.Assets = new List<Asset>();
            this.MarketOrders = new List<MarketOrder>();
        }

        public long ID { get; set; }
        public string Name { get; set; }
        public int SolarSystemID { get; set; }
        public decimal Tax { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<MarketOrder> MarketOrders { get; set; }
        public virtual SolarSystem SolarSystem { get; set; }
    }
}
