namespace DBSoft.EVEAPI.Entities.IndustryJob
{
    using System;
    using System.Threading.Tasks;
    using Account;
    using WalletTransaction;

    public class MockIndustryJobService : IIndustryJobService
	{
        public async Task<ApiLoadResponse<IndustryJob>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId)
		{
            var result = await Task.Run(() => new ApiLoadResponse<IndustryJob>
            {
                CachedUntil = DateTime.Now.AddHours(1)
            });
            return result;
		}
	}
}
