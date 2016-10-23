namespace DBSoft.EPM.DAL.Requests
{
    public class SsoAuthenticateRequest
    {
        public string EveOnlineCharacter { get; set; }
        public string IpAddress { get; set; }
        public string RefreshToken { get; set; }
        public string SessionToken { get; set; }
    }
}