namespace DBSoft.EPM.Logic
{
    using System.Threading.Tasks;
    using DAL.Services.AccountApi;
    using System.Collections.Generic;
	using AutoMapper;
	using DAL.Annotations;
	using DAL.CodeFirst.Models;
	using DAL.Requests;
	using EVEAPI.Entities.Account;
	using System.Linq;

    [UsedImplicitly]
    public class EveAccountProcessor : IEveAccountProcessor
    {
		private readonly IApiKeyInfoService _apiKeyInfoService;
        private readonly IAccountService _accountService;

        public EveAccountProcessor(IApiKeyInfoService apiKeyInfoService, IAccountService accounts)
		{
			_apiKeyInfoService = apiKeyInfoService;
            _accountService = accounts;
		}
		public async Task<AccountDTO> SaveAccount(AccountDTO account, string token)
		{
			var result = Mapper.Map<AccountDTO>(account);
			if (account.DeletedFlag)
			{
				_accountService.DeleteAccount(new DeleteAccountRequest { AccountID = account.AccountID, Token = token });
				return result;
			}

			var info = await _apiKeyInfoService.Load(account.ApiKeyID, account.ApiVerificationCode);
            var first = info.First();

			var request = new SaveAccountRequest
			{
				Token = token,
				AccountID = account.AccountID,
				AccountName = account.AccountName,
				ApiKeyID = account.ApiKeyID,
				ApiVerificationCode = account.ApiVerificationCode,
				ApiAccessMask = first.AccessMask,
				ApiKeyType = (AccountType) first.ApiKeyType,
				DeletedFlag = false
			};
			
			if (first.ApiKeyType == ApiKeyType.Character )
			{
				request.Characters = new List<SaveCharacterRequest>();
				foreach (var character in first.Characters)
				{
					request.Characters.Add(new SaveCharacterRequest
						{
							EveCharacterID = character.ID,
							CharacterName = character.Name
						});
				}
			}
			else
			{
				request.Corporations = new List<SaveCorporationRequest>
				{
					new SaveCorporationRequest
					{
						EveCorporationID = first.Characters.First().CorporationID,
						CorporationName = first.Characters.First().CorporationName
					}
				};
			}
			_accountService.SaveAccount(request);
			result.ApiAccessMask = first.AccessMask;
			result.ApiKeyType = (AccountType)first.ApiKeyType;
			return result;
		}
	}
}
