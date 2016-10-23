namespace DBSoft.EVEAPI.Entities.Account
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

    public interface IApiKeyInfoService
	{
		Task<IEnumerable<ApiKeyInfo>> Load(int keyId, string vCode);
	}
}