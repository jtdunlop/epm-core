namespace DBSoft.EPM.DAL.Requests
{
	using System.Collections.Generic;

	public class AccountBalanceUpdateRequest
	{
		public List<AccountBalanceUpdateRequestItem> BalanceUpdates { get; set; }
	}
}