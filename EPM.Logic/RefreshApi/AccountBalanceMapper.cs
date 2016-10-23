namespace DBSoft.EPM.Logic.RefreshApi
{
    using System.Threading.Tasks;
    using DAL.Annotations;
    using DAL.Services.AccountApi;
    using DAL.Interfaces;
	using DAL.Requests;
    using EVEAPI.Entities.AccountBalance;
    using EVEAPI.Entities.WalletTransaction;
	using System;
	using System.Linq;
    using IAccountBalanceService = DAL.Interfaces.IAccountBalanceService;
    using IEveAccountBalanceService = EVEAPI.Entities.AccountBalance.IAccountBalanceService;

    [UsedImplicitly]
    public class AccountBalanceMapper : EveApiMapper, IAccountBalanceMapper
    {
		private readonly IAccountBalanceService _service;
        private readonly IEveAccountBalanceService _balanceService;
        private readonly IAccountApiService _accounts;

	    public AccountBalanceMapper(IAccountBalanceService service,  IEveAccountBalanceService balanceService,
            IEveApiStatusService statusService, IAccountApiService accounts) : base(statusService)
		{
			_service = service;
	        _balanceService = balanceService;
	        _accounts = accounts;
		}

		public async Task Pull(string token)
		{
			const string serviceName = "AccountBalance";
            
			var cachedUntil = DateTime.Now;
		    try
		    {
                foreach (var account in _accounts.List(token))
                {
                    var response = await _balanceService.Load(account.ApiKeyType, account.ApiKeyID, account.ApiVerificationCode, account.EveApiID);
                    cachedUntil = response.CachedUntil;
                    ProcessBalances(response, account.AccountID);
                }
            }
            catch (Exception e)
            {
                SaveError(serviceName, token, e.Message);
                throw;
            }
            UpdateStatus(serviceName, cachedUntil, token);
		}

		private void ProcessBalances(ApiLoadResponse<AccountBalance> response, int accountID)
		{
			var eveBalances = response.Data;
			var request = new AccountBalanceUpdateRequest
			{
				BalanceUpdates = eveBalances.Select(eveBalance => new AccountBalanceUpdateRequestItem
				{
					AccountID = accountID, 
					AccountKey = eveBalance.AccountKey, 
					Balance = eveBalance.Balance
				}).ToList()
			};
			_service.UpdateBalances(request);

		}
	}
}
