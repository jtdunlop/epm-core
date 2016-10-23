namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMarketService
    {
        MarketOrderDTO ListOrders(MarketOrderRequest request);
        List<MarketHistoryDTO> ListHistory(MarketHistoryRequest request);
        Task<List<MarketSummaryDTO>> ListSummaries(MarketSummaryRequest request);
    }
}