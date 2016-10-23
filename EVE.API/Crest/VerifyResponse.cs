namespace DBSoft.EVEAPI.Crest
{
    using System;

    public class VerifyResponse
    {
        public int CharacterId { get; set; }
        public string CharacterName { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Scopes { get; set; }
        public string TokenType { get; set; }
        public string CharacterOwnerHash { get; set; }
    }
}