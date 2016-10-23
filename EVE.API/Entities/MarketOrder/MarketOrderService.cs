namespace DBSoft.EVEAPI.Entities.MarketOrder
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Account;
	using Plumbing;
	using WalletTransaction;

    public class MarketOrderService : IMarketOrderService
    {
        private readonly IEveApiLoader _loader;

        public MarketOrderService(IEveApiLoader loader)
        {
            _loader = loader;
        }

        public async Task<ApiLoadResponse<MarketOrder>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId)
        {
            var kt = keyType == ApiKeyType.Character ? "char" : "corp";
            var url =
                $"http://api.eve-online.com/{kt}/MarketOrders.xml.aspx?keyID={apiKeyId}&vCode={vCode}&characterID={eveApiId}";
            var response = await _loader.Load(url);
            if (!response.Success) throw new Exception(response.ErrorMessage);
            var result = new ApiLoadResponse<MarketOrder>
            {
                CachedUntil = response.CachedUntil
            };
            var element = response.Result.Element("rowset");
            if (element != null)
                result.Data.AddRange(element.Elements("row").Select(f => new MarketOrder
					{
						ID = (long) f.Attribute("orderID"),
						CharacterID = (int) f.Attribute("charID"),
						StationID = (int) f.Attribute("stationID"),
						VolumeEntered = (int) f.Attribute("volEntered"),
						VolumeRemaining = (int) f.Attribute("volRemaining"),
						MinimumVolume = (int) f.Attribute("minVolume"),
						OrderState = (OrderState) (int) f.Attribute("orderState"),
						ItemID = (int) f.Attribute("typeID"),
						Range = (short) f.Attribute("range"),
						AccountKey = (int) f.Attribute("accountKey"),
						Duration = (int) f.Attribute("duration"),
						Escrow = (decimal) f.Attribute("escrow"),
						Price = (decimal) f.Attribute("price"),
						OrderType = (bool) f.Attribute("bid") ? OrderType.Buy : OrderType.Sell,
						WhenIssued = (DateTime) f.Attribute("issued")
					}));
			return result;
		}
	}
}
