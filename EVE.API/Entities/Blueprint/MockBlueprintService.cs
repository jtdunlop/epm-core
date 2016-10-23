namespace DBSoft.EVEAPI.Entities.Blueprint
{
    using System;
    using System.Threading.Tasks;
    using Account;
    using System.Linq;
    using Plumbing;
    using WalletTransaction;

    public class MockBlueprintService : IBlueprintService
	{
        public async Task<ApiLoadResponse<Blueprint>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId)
        {
            var result = await Task.Run(() => new ApiLoadResponse<Blueprint>
            {
                CachedUntil = DateTime.Now.AddHours(1)
            });
            return result;
        }
	}
}