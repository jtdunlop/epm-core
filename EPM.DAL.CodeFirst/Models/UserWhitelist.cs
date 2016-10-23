namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class UserWhitelist
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public bool Enabled { get; set; }
    }
}
