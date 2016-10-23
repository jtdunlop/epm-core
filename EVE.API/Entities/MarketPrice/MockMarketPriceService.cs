namespace DBSoft.EVEAPI.Entities.MarketPrice
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DbSoft.Cache.Aspect.Attributes;
    using EveAPI.Crest.Root;
    using Newtonsoft.Json;

    public class MockMarketPriceService : IMarketPriceService
    {
        [Cache.Cacheable]
        public async Task<IEnumerable<MarketPriceDTO>> GetMarketPrices()
        {
            var url = CrestService.GetPublicEndpoints().MarketPrices.Href;
            var json = await JsonLoader.LoadAsync(url);
            var result = JsonConvert.DeserializeObject<RootObject>(json);
            return result.items.Select(f => new MarketPriceDTO
            {
                ItemId = f.type.id,
                ItemName = f.type.name,
                AdjustedPrice = (decimal)f.adjustedPrice
            }).ToList();
        }

    }
}