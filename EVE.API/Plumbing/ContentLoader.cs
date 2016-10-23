namespace DBSoft.EVEAPI.Plumbing
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class ContentLoader : IEveContentLoader
    {
        public async Task<string> LoadContent(string url)
        {
            var client = new HttpClient();
            var resp = await client.GetAsync(new Uri(url));
            var content = await resp.Content.ReadAsStringAsync();
            return content;
        }
    }
}