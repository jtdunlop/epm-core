namespace DBSoft.EveAPI.Crest.Root
{
    using DbSoft.Cache.Aspect.Attributes;
    using EVEAPI.Entities;
    using Newtonsoft.Json;

    public class CrestService
    {
        [Cache.Cacheable]
        public static EndpointRoot GetPublicEndpoints()
        {
            const string url = "https://public-crest.Eveonline.com";

            return GetEndpoints(url);
        }

        [Cache.Cacheable]
        public static EndpointRoot GetAuthEndpoints()
        {
            const string url = "https://crest-tq.Eveonline.com";

            return GetEndpoints(url);
        }

        private static EndpointRoot GetEndpoints(string url)
        {
            var readToEnd = JsonLoader.Load(url);
            var result = JsonConvert.DeserializeObject<EndpointRoot>(readToEnd);
            return result;
        }
    }
}