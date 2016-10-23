namespace DBSoft.EVEAPI.Entities.Blueprint
{
    using System.Threading.Tasks;
    using Account;
    using WalletTransaction;

    public interface IBlueprintService
    {
        Task<ApiLoadResponse<Blueprint>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId);
    }
}