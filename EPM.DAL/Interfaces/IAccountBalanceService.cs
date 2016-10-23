namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using DTOs;
    using Requests;

    public interface IAccountBalanceService
	{
		IEnumerable<AccountBalanceDTO> List(string token);
		void UpdateBalances(AccountBalanceUpdateRequest request);
	}
}