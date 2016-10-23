namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Annotations;

    [UsedImplicitly]
    public class MockMarketService : IMarketService
    {
        public MarketOrderDTO ListOrders(MarketOrderRequest request)
        {
            //return request.ItemIds.Select(item => new Order
            //{
            //    ItemID = item,
            //    StationID = 60008170
            //}).ToList();
            return null;
        }

        public List<MarketHistoryDTO> ListHistory(MarketHistoryRequest request)
        {
            return request.ItemIds.Select(item => new MarketHistoryDTO
            {
                ItemID = item,
                AverageDailySales = 1
            }).ToList();
        }

        public Task<List<MarketSummaryDTO>> ListSummaries(MarketSummaryRequest request)
        {
            return Task.FromResult(request.ItemIDs.Select(item => new MarketSummaryDTO
            {
                ItemID = item,
                Competitors = 1,
                SellVolume = 1,
                MinimumSellPrice = 1,
                MaximumBuyPrice = 1
            }).ToList());
        }
    }
}