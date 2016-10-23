namespace DBSoft.EVEAPI.Entities.Facility
{
    public class Item
    {
        public int facilityID { get; set; }
        public SolarSystem solarSystem { get; set; }
        public string name { get; set; }
        public Region region { get; set; }
        public double tax { get; set; }
        public string facilityID_str { get; set; }
        public Owner owner { get; set; }
        public Type type { get; set; }
    }
}