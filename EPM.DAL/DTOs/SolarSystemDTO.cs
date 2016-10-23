namespace DBSoft.EPM.DAL.DTOs
{
    public class SolarSystemDTO
    {
        public int SolarSystemID { get; set; }
        public string SolarSystemName { get; set; }
        public decimal ManufacturingCost { get; set; }
        public decimal InstallationTax { get; set; }
        public int RegionID { get; set; }
    }
}