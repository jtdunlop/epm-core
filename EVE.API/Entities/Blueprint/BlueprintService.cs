namespace DBSoft.EVEAPI.Entities.Blueprint
{
    using System;
    using System.Threading.Tasks;
    using Account;
    using System.Linq;
    using Plumbing;
    using WalletTransaction;

    public class BlueprintService : IBlueprintService
    {
        private readonly IEveApiLoader _loader;

        public BlueprintService(IEveApiLoader loader)
        {
            _loader = loader;
        }

        public async Task<ApiLoadResponse<Blueprint>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId)
        {
            var kt = keyType == ApiKeyType.Character ? "char" : "corp";
            var url =
                $"http://api.eve-online.com/{kt}/Blueprints.xml.aspx?keyID={apiKeyId}&vCode={vCode}&characterID={eveApiId}";
            var response = await _loader.Load(url);
            if (!response.Success) throw new Exception(response.ErrorMessage);
            var result = new ApiLoadResponse<Blueprint>
            {
                CachedUntil = response.CachedUntil
            };
            var element = response.Result.Element("rowset");
            if (element != null)
                result.Data.AddRange(element.Elements("row").Select(row => new Blueprint
                {
                    AssetID = long.Parse(row.Attribute("itemID").Value), 
                    BlueprintID = long.Parse(row.Attribute("typeID").Value),
                    IsCopy = int.Parse(row.Attribute("quantity").Value) == -2, 
                    MaterialEfficiency = int.Parse(row.Attribute("materialEfficiency").Value), 
                    TimeEfficiency = int.Parse(row.Attribute("timeEfficiency").Value)
                }));
            return result;
        }
	}
}