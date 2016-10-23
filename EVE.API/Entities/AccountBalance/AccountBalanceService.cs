namespace DBSoft.EVEAPI.Entities.AccountBalance
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Account;
	using Plumbing;
	using WalletTransaction;

	public class AccountBalanceService : IAccountBalanceService
	{
	    private readonly IEveApiLoader _loader;

	    public AccountBalanceService(IEveApiLoader loader)
	    {
	        _loader = loader;
	    }

	    public async Task<ApiLoadResponse<AccountBalance>> Load(ApiKeyType keyType, int keyId, string vCode, int eveApiId)
		{
			var url =
			    $"http://api.eve-online.com/{keyType.ServiceBase()}/AccountBalance.xml.aspx?keyID={keyId}&vCode={vCode}&characterID={eveApiId}";
			var result = await _loader.Load(url);

			if (!result.Success)
			{
				throw new Exception(result.ErrorMessage);
			}
			var xElement = result.Result.Element("rowset");
			var response = new ApiLoadResponse<AccountBalance>
			{
				Data = new List<AccountBalance>(),
				CachedUntil = result.CachedUntil
			};
			if (xElement != null)
			{
				response.Data = xElement.Elements("row").Select(f => new AccountBalance
				{
					AccountID = (int) f.Attribute("accountID"),
					AccountKey = (int) f.Attribute("accountKey"),
					Balance = (decimal) f.Attribute("balance")
				}).ToList();
			}
			return response;
		}
	}
	public static class ApiExtensions
	{
		public static string ServiceBase(this ApiKeyType keyType)
		{
			return (keyType == ApiKeyType.Character ? "char" : "corp");
		}
	}

	public interface IAccountBalanceService
	{
		Task<ApiLoadResponse<AccountBalance>> Load(ApiKeyType corporation, int keyId, string apiVerificationCode, int eveApiId);
	}
}
