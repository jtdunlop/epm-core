namespace DBSoft.EVEAPI.Entities.IndustryJob
{
    using System.Threading.Tasks;
    using Account;
    using WalletTransaction;

    public interface IIndustryJobService
    {
        Task<ApiLoadResponse<IndustryJob>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId);
    }
}