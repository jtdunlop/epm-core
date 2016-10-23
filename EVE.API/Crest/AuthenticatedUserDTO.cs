namespace DBSoft.EVEAPI.Crest
{
    public class AuthenticatedUserDTO
    {
        public string Token { get; set; }
        public string CharacterName { get; set; }
        public string RefreshToken { get; set; }
    }
}