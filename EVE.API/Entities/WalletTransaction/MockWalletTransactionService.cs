namespace DBSoft.EVEAPI.Entities.WalletTransaction
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Account;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class MockWalletTransactionService : IWalletTransactionService
    {
        public async Task<ApiLoadResponse<WalletTransaction>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId, int maxAge, long lastTransactionId)
        {
            var result = await Task.Run(() => new ApiLoadResponse<WalletTransaction>
            {
                Data = new List<WalletTransaction>(),
                CachedUntil = DateTime.Now.AddHours(1)
            });
            return result;
        }
    }
}