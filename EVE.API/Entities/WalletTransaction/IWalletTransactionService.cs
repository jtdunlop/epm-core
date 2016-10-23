namespace DBSoft.EVEAPI.Entities.WalletTransaction
{
    using System.Threading.Tasks;
    using Account;

	public interface IWalletTransactionService
	{
		Task<ApiLoadResponse<WalletTransaction>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId, int maxAge, long lastTransactionId);
	}
}