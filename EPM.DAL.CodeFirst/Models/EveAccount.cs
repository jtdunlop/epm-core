namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public class EveAccount
    {
        public int ID { get; set; }
        public string EveCharacterName { get; set; }
        public int UserID { get; set; }

        public virtual User User { get; set; }
    }
}
