namespace DBSoft.EVEAPI.Entities.MarketOrder
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Account;
    using Plumbing;
    using WalletTransaction;

    public class MockMarketOrderService : IMarketOrderService
    {
        public async Task<ApiLoadResponse<MarketOrder>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId)
        {
           var result = await Task.Run(() => new ApiLoadResponse<MarketOrder>
            {
                CachedUntil = DateTime.Now.AddHours(1)
            });
            return result;
        }
    }
}