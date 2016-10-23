namespace DBSoft.EVEAPI.Entities.Facility
{
    public class FacilityDTO
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int SolarSystemId { get; set; }
        public decimal Tax { get; set; }
    }
}