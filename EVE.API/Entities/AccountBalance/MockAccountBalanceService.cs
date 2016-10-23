namespace DBSoft.EVEAPI.Entities.AccountBalance
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Account;
    using WalletTransaction;

    public class MockAccountBalanceService : IAccountBalanceService
	{
		public async Task<ApiLoadResponse<AccountBalance>> Load(ApiKeyType keyType, int keyId, string vCode, int eveApiId)
		{
		    return await Task.Run(() =>
		    {
		        var accounts = new List<AccountBalance>
		        {
                    new AccountBalance
                    {
                        Balance = 50000000m
                    }
		        };
                var response = new ApiLoadResponse<AccountBalance>
                {
                    Data = accounts,
                    CachedUntil = DateTime.Now.AddHours(1)
                };
                return response;
            });
		}
	}
}
