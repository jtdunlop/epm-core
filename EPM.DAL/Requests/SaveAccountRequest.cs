namespace DBSoft.EPM.DAL.Requests
{
	using System.Collections.Generic;
	using CodeFirst.Models;

	public class SaveAccountRequest
	{
		public string Token { get; set; }
		public int AccountID { get; set; }
		public string AccountName { get; set; }
		public int ApiKeyID { get; set; }
		public string ApiVerificationCode { get; set; }
		public AccountType ApiKeyType { get; set; }
		public bool DeletedFlag { get; set; }
		public List<SaveCharacterRequest> Characters { get; set; }
		public List<SaveCorporationRequest> Corporations { get; set; }
		public int ApiAccessMask { get; set; }
	}
}