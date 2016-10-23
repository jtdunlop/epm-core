namespace DBSoft.EPM.DAL.DTOs
{
	public class CorporationAccountDTO
	{
		public int CorporationID { get; set; }
		public int AccountID { get; set; }
		public int ApiKeyID {get;set;}
		public string ApiVerificationCode {get;set;}
		public int EveApiID {get;set;}
	}
}