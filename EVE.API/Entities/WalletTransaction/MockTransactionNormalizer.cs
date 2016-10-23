namespace DBSoft.EVEAPI.Entities.WalletTransaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class MockTransactionNormalizer : ITransactionNormalizer
    {
        public void Normalize(List<WalletTransaction> transactions)
        {
            var date = DateTime.UtcNow.AddDays(-1);

            if (!transactions.Any() || transactions.Any(f => f.DateTime > date)) return;
            var max = transactions.Max(f => f.DateTime);
            var dif = (date - max).Days;
            transactions.ForEach(f => f.DateTime = f.DateTime.AddDays(dif));
        }
    }
}