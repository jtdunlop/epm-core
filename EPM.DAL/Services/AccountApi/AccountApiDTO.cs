using DBSoft.EVEAPI.Entities.Account;

namespace DBSoft.EPM.DAL.Services.AccountApi
{
    public class AccountApiDTO
    {
        public int ApiID { get; set; }
        public ApiKeyType ApiKeyType { get; set; }
        public int AccountID { get; set; }
        public int EveApiID { get; set; }
        public int ApiKeyID { get; set; }
        public string ApiVerificationCode { get; set; }
        public string AccountName { get; set; }
    }
}