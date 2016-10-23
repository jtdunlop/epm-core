namespace DBSoft.EPM.DAL.DTOs
{
    public class ItemTransactionBySubscriberDto : ItemTransactionBaseDto
    {
        public int UserId { get; set; }
        public string EveOnlineCharacter { get; set; }
    }
}