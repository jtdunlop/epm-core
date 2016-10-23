namespace DBSoft.EPM.DAL.DTOs
{
	public class CharacterDTO
	{
		public int CharacterID { get; set; }
		public int AccountID { get; set; }
		public int EveApiID { get; set; }
		public int ApiKeyID { get; set; }
		public string ApiVerificationCode { get; set; }
		public string AccountName { get; set; }
	}
}