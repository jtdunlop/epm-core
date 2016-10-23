using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class SolarSystem
    {
        public SolarSystem()
        {
            Stations = new List<Station>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int RegionID { get; set; }
        public decimal ManufacturingCost { get; set; }
        public decimal InstallationTax { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<Station> Stations { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public ICollection<Gate> Gates { get; set; }
    }
}
