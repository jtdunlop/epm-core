namespace DBSoft.EVEAPI.Entities.WalletTransaction
{
    using System.Collections.Generic;

    public class NoopTransactionNormalizer : ITransactionNormalizer
    {
        public void Normalize(List<WalletTransaction> transactions)
        { }
    }
}