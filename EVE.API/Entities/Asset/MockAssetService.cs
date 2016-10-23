namespace DBSoft.EVEAPI.Entities.Asset
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Account;
    using WalletTransaction;

    public class MockAssetService : IAssetService
    {
        private const int RavenBlueprint = 688;
        private const int Orkashu = 60008170;

        public async Task<ApiLoadResponse<Asset>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId)
        {
            var result = await Task.Run(() => new ApiLoadResponse<Asset>
            {
                Data = new List<Asset>
                {
                    new Asset
                    {
                        ItemID = RavenBlueprint,
                        LocationID = Orkashu,
                        Quantity = 1
                    }
                },
                CachedUntil = DateTime.Now.AddHours(1)
            });
            return result;
        }
    }
}