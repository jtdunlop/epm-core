namespace DBSoft.EVEAPI.Plumbing
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class MockContentLoader : IEveContentLoader
    {
        private bool _done;
        public async Task<string> LoadContent(string url)
        {
            if (url.Contains("/char/AssetList.xml.aspx?"))
            {
                var resource = await LoadResource("DBSoft.EVEAPI.Plumbing.charassets.xml");
                return resource;
            }
            if (url.Contains("/corp/AssetList.xml.aspx?"))
            {
                var resource = await LoadResource("DBSoft.EVEAPI.Plumbing.corpassets.xml");
                return resource;
            }
            if (url.Contains("/char/AccountBalance.xml.aspx?"))
            {
                var resource = await LoadResource("DBSoft.EVEAPI.Plumbing.charbalance.xml");
                return resource;
            }
            if (url.Contains("/corp/AccountBalance.xml.aspx?"))
            {
                var resource = await LoadResource("DBSoft.EVEAPI.Plumbing.corpbalance.xml");
                return resource;
            }
            if (url.Contains("/corp/WalletTransactions.xml.aspx?"))
            {
                var resource = _done
                    ? await LoadResource("DBSoft.EVEAPI.Plumbing.emptytransactions.xml")
                    : await LoadResource("DBSoft.EVEAPI.Plumbing.transactions.xml");
                _done = true;
                return resource;
            }
            if (url.Contains("/char/WalletTransactions.xml.aspx?"))
            {
                return await LoadResource("DBSoft.EVEAPI.Plumbing.emptytransactions.xml");
            }
            if (url.Contains("/IndustryJobsHistory.xml.aspx?"))
            {
                var resource = await LoadResource("DBSoft.EVEAPI.Plumbing.jobs.xml");
                return resource;
            }
            if (url.Contains("/Blueprints.xml.aspx?"))
            {
                var resource = await LoadResource("DBSoft.EVEAPI.Plumbing.blueprints.xml");
                return resource;
            }
            if (url.Contains("/MarketOrders.xml.aspx?"))
            {
                var resource = await LoadResource("DBSoft.EVEAPI.Plumbing.orders.xml");
                return resource;
            }
            throw new Exception($"Not implemented: {url}");
        }

        private async Task<string> LoadResource(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resource))
            {
                if (stream == null)
                {
                    return null;
                }
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}