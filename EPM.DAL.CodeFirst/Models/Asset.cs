using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class Asset
    {
        public Asset()
        {
            this.BlueprintInstances = new List<BlueprintInstance>();
            this.ProductionJobs = new List<ProductionJob>();
        }

        public long ID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public int AccountID { get; set; }
        public int? ContainerID { get; set; }
        public bool DeletedFlag { get; set; }
        public long? StationID { get; set; }
        public int? SolarSystemID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public virtual Account Account { get; set; }
        public virtual Item Item { get; set; }
        public virtual Station Station { get; set; }
        public virtual SolarSystem SolarSystem { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BlueprintInstance> BlueprintInstances { get; set; }
        public virtual ICollection<ProductionJob> ProductionJobs { get; set; }
    }
}
