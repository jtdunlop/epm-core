using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class Region
    {
        public Region()
        {
            this.SolarSystems = new List<SolarSystem>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SolarSystem> SolarSystems { get; set; }
    }
}
