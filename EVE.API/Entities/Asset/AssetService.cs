namespace DBSoft.EVEAPI.Entities.Asset
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using System.Xml.Linq;
	using Account;
	using Plumbing;
	using WalletTransaction;

    public class AssetService : IAssetService
    {
        private readonly IEveApiLoader _loader;

        public AssetService(IEveApiLoader loader)
        {
            _loader = loader;
        }

        public async Task<ApiLoadResponse<Asset>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId)
		{
            var kt = keyType == ApiKeyType.Character ? "char" : "corp";
            var service =
                $"http://api.eve-online.com/{kt}/AssetList.xml.aspx?keyID={apiKeyId}&vCode={vCode}&characterID={eveApiId}";
            var response = await _loader.Load(service);
		    if (!response.Success) throw new Exception(response.ErrorMessage);
            var result = new ApiLoadResponse<Asset>
			{
				Data = new List<Asset>(),
                CachedUntil = response.CachedUntil
			};
		    RecursiveLoad(result.Data, response.Result.Element("rowset"), (int?)response.Result.Attribute("locationID"));
		    return result;
		}

		// Load flattened list of assets
		private static void RecursiveLoad(ICollection<Asset> assets, XContainer element, int? parentLocation)
		{
			if ( element == null )
			{
				return;
			}
			foreach ( var row in element.Elements("row") )
			{
				var locationId = (int?)row.Attribute("locationID");
				if ( !locationId.HasValue )
				{
					locationId = parentLocation;
				}
				var asset = new Asset
				{
					ID = (long)row.Attribute("itemID"),
					ItemID = (int)row.Attribute("typeID"),
					Quantity = (int)row.Attribute("quantity"),
					LocationID = locationId,
					IsPackaged = (int)row.Attribute("singleton") == 0
				};
				RecursiveLoad(assets, row.Element("rowset"), locationId);
				assets.Add(asset);
			}
		}
	}
}