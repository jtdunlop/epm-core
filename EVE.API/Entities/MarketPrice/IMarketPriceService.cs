namespace DBSoft.EVEAPI.Entities.MarketPrice
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMarketPriceService
    {
        Task<IEnumerable<MarketPriceDTO>> GetMarketPrices();
    }
}