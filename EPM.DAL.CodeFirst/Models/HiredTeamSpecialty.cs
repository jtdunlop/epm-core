namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    namespace DBSoft.EPM.DAL.CodeFirst.Models
    {
        public class HiredTeamSpecialty
        {
            public int ID { get; set; }
            public int HiredTeamID { get; set; }
            public string SpecialtyName { get; set; }
            public decimal Bonus { get; set; }

            public virtual HiredTeam HiredTeam { get; set; }
        }
    }
}
