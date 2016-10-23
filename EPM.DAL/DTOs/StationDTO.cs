namespace DBSoft.EPM.DAL.DTOs
{
    public class StationDTO
	{
		public long StationID { get; set; }
		public string StationName { get; set; }
	    public int SolarSystemID { get; set; }
	    public decimal StationTax { get; set; }
	}
}