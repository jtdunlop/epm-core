namespace DBSoft.EVEAPI.Entities.MarketOrder
{
    using System.Threading.Tasks;
    using Account;
    using WalletTransaction;

    public interface IMarketOrderService
    {
        Task<ApiLoadResponse<MarketOrder>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId);
    }
}