namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public class HiredTeamCost
    {
        public int ID { get; set; }
        public int HiredTeamID { get; set; }
        public int TransactionID { get; set; }

        public virtual HiredTeam HiredTeam { get; set; }
        public virtual Transaction Transaction { get; set; } 
    }
}
