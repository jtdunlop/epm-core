namespace DBSoft.EVEAPI.Entities.Asset
{
    using System.Threading.Tasks;
    using Account;
    using WalletTransaction;

    public interface IAssetService
    {
        Task<ApiLoadResponse<Asset>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId);
    }
}