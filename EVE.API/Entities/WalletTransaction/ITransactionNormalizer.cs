namespace DBSoft.EVEAPI.Entities.WalletTransaction
{
    using System.Collections.Generic;

    public interface ITransactionNormalizer
    {
        void Normalize(List<WalletTransaction> transactions);
    }
}