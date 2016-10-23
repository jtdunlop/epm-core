namespace DBSoft.EVEAPI.Entities
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class JsonLoader
    {
        public static string Load(string url, string token = null)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.UnsafeAuthenticatedConnectionSharing = true;
            req.Proxy = WebRequest.DefaultWebProxy;
            var p = req.Proxy;
            req.PreAuthenticate = true;
            req.Method = "GET";
            req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            var v1 = ServicePointManager.UseNagleAlgorithm;
            var v2 = ServicePointManager.DefaultConnectionLimit;
            var v3 = ServicePointManager.Expect100Continue;

            var response = req.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                if (stream == null) return null;
                using (var buffer = new BufferedStream(stream))
                {
                    var reader = new StreamReader(buffer);
                    return reader.ReadToEnd();
                }
            }
        }

        public async static Task<string> LoadAsync(string url, string token = null)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Bearer", token);
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }
    }
}