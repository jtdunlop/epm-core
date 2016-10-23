namespace DBSoft.EVEAPI.Plumbing
{
    using System;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class EveApiLoader : IEveApiLoader
    {
        private readonly IEveContentLoader _loader;

        public EveApiLoader(IEveContentLoader loader)
        {
            _loader = loader;
        }

        public async Task<EveApiLoaderResponse> Load(string url)
        {
            var response = new EveApiLoaderResponse();

            var content = await _loader.LoadContent(url);

            var xml = XDocument.Parse(content);
            var api = xml.Element("eveapi");
            if (api == null) return response;
            var xElement = api.Element("cachedUntil");
            if (xElement != null) response.CachedUntil = DateTime.Parse(xElement.Value);
            var element = api.Element("currentTime");
            if (element != null) response.CurrentTime = DateTime.Parse(element.Value);
            var error = api.Element("error");
            if (error != null)
            {
                response.ErrorCode = int.Parse(error.Attribute("code").Value);
                response.ErrorMessage = error.Value;
            }
            else
            {
                response.Success = true;
                response.Result = api.Element("result");
            }
            return response;
        }
    }
}