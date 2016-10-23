namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public class HiredTeamAuction
    {
        public int ID { get; set; }
        public int HiredTeamID { get; set; }
        public int UserID { get; set; }
        public decimal AuctionCost { get; set; }

        public virtual HiredTeam HiredTeam { get; set; }
        public virtual User User { get; set; }
    }
}
