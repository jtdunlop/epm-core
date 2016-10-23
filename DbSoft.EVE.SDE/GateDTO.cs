namespace DBSoft.EVE.SDE
{
    public class GateDTO
    {
        public int FromRegionId { get; set; }
        public int FromConstellationId { get; set; }
        public int FromSolarSystemId { get; set; }
        public int ToSolarSystemId { get; set; }
        public int ToConstellationId { get; set; }
        public int ToRegionId { get; set; }
    }
}