namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public class Gate
    {
        public int ID { get; set; }
        public int SolarSystemID { get; set; }
        public int TargetSolarSystemID { get; set; }
        public virtual SolarSystem SolarSystem { get; set; }
        public virtual SolarSystem TargetSolarSystem { get; set; }
    }
}
