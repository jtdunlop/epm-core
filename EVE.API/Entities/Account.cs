namespace DBSoft.EVEAPI
{
	public class Account
	{
		public Account(int keyID, string vCode)
		{
			KeyID = keyID;
			VCode = vCode;
			ApiKeyInfo = new ApiKeyInfo(keyID, vCode);
		}

		public int KeyID { get; set; }
		public string VCode { get; set; }
		public ApiKeyInfo ApiKeyInfo { get; private set; }
	}
}
