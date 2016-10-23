namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DbSoft.Cache.Aspect.Attributes;
    using Entities;
    using Entities.MarketOrder;
    using EveAPI.Crest.Root;
    using Inventory;
    using Newtonsoft.Json;
    using ItemTypes = Inventory.ItemTypes;

    public class MarketService : IMarketService
    {
        private const string BaseUri = "https://crest-tq.eveonline.com";

        public MarketOrderDTO ListOrders(MarketOrderRequest request)
        {
            var listOfLists = request.ItemIds.ToList();
            var tasks = listOfLists.Select(batch => Task.Run(() => LoadMarketOrders(request, batch))).ToList();
            var dto = new MarketOrderDTO {Orders = new List<Order>()};
            var result = Task.WhenAll(tasks);
            foreach (var r in result.Result)
            {
                dto.Orders.AddRange(r);
                foreach (var c in from orders in result.Result from c in orders where c.StationID > 99999999 select c)
                {
                    dto.Citadels.Add(new Citadel
                    {
                        Id = c.StationID, 
                        Name = c.StationName
                    });
                }
            }
            return dto;
        }

        private static List<Order> LoadMarketOrders(MarketOrderRequest request, int id)
        {
            var url = string.Format("{3}/market/{0}/orders/{1}/?type=https://crest-tq.eveonline.com/inventory/types/{2}/",
                     request.RegionId, request.OrderType.ToString().ToLower(), id, BaseUri);
            var readToEnd = JsonLoader.Load(url, request.Token);
            var result = JsonConvert.DeserializeObject<MarketOrderResponse>(readToEnd);
            if ( id == 34 )
            {
                int a = 4;
            }
            var list = result.Items.Select(f => new Order
            {
                ItemID = f.type.id,
                OrderID = f.id,
                OrderType = f.buy ? OrderType.Buy : OrderType.Sell,
                Price = f.price,
                StationID = f.location.id,
                StationName= f.location.name,
                Range = GetRange(f.range)
            }).ToList();
            return list;
        }

        public List<MarketHistoryDTO> ListHistory(MarketHistoryRequest request)
        {
            var listOfLists = request.ItemIds.ToList();
            var tasks = listOfLists.Select(id => Task.Run(() => LoadMarketHistory(request, id))).ToList();
            var result = Task.WhenAll(tasks);
            return result.Result.ToList();
        }

        public async Task<List<MarketSummaryDTO>> ListSummaries(MarketSummaryRequest request)
        {
            var ids = request.ItemIDs.ToList();
            var summaries = new List<MarketSummaryDTO>();
            foreach ( var id in ids )
            {
                var dto = await LoadMarketSummaries(request, id);
                if ( dto != null )
                {
                    summaries.Add(dto);
                }
            }
            return summaries;
        }

        [Cache.Cacheable]
        private static List<MarketOrderItem> LoadBulkMarketOrders(MarketSummaryRequest request)
        {
            var page = 1;
            int lastPage;
            var list = new List<MarketOrderItem>();
            do
            {
                var url = $"https://crest-tq.eveonline.com/market/{request.RegionId}/orders/all/?page={page}";
                var readToEnd = JsonLoader.Load(url);
                var result = JsonConvert.DeserializeObject<BulkMarketOrderResponse>(readToEnd);
                list.AddRange(result.Items);
                lastPage = result.PageCount;
                page++;
            } while (page <= lastPage);
            return list;
        }

        [Cache.Cacheable]
        private static async Task<List<ItemType>> GetItemTypes()
        {
            var page = 1;
            int lastPage;
            var list = new List<ItemType>();
            do
            {
                var url = CrestService.GetAuthEndpoints().ItemTypes.Href + $"?page={page}";
                var readToEnd = await JsonLoader.LoadAsync(url);
                var result = JsonConvert.DeserializeObject<ItemTypes>(readToEnd);
                list.AddRange(result.items);
                lastPage = result.pageCount;
                page++;
            } while (page <= lastPage);

            return list;
        }


        private static async Task<MarketSummaryDTO> LoadMarketSummaries(MarketSummaryRequest request, int id)
        {
            var result = LoadBulkMarketOrders(request);
            var list = request.StationID == 0 ?
                result.Where(f => f.Type == id).ToList() : 
                result.Where(f => f.Type == id && f.StationID == request.StationID).ToList();
            if ( !list.Any() || !list.Any(f => f.Buy) || list.All(f => f.Buy))
            {
                return null;
            }
            var types = await GetItemTypes();
            if ( types.All(f => f.id != id) )
            {
                return null;
            }
            var dto = new MarketSummaryDTO
            {
                ItemID = id,
                ItemName = types.First(f => f.id == id).name,
                MinimumSellPrice = request.OrderType == OrderType.Sell ?
                    (decimal)list.Where(f => !f.Buy).Min(f => f.Price) : 0,
                SellVolume = list.Sum(f => f.Volume),
                MaximumBuyPrice = request.OrderType == OrderType.Buy ?
                    (decimal)list.Where(f => f.Buy).Max(f => f.Price) : 0,
                Competitors = Math.Max(list.Where(IsActive).Count(), 1),
            };
            return dto;
        }

        private static bool IsActive(MarketOrderItem item)
        {
            return DateTime.Parse(item.Issued) > DateTime.UtcNow.AddDays(-1);
        }

        private static MarketHistoryDTO LoadMarketHistory(MarketHistoryRequest request, int id)
        {
            var url = $"{BaseUri}/market/{request.RegionID}/history/?type=https://crest-tq.eveonline.com/inventory/types/{id}/";
            try
            {
                var readToEnd = JsonLoader.Load(url);
                var result = JsonConvert.DeserializeObject<MarketHistoryResponse>(readToEnd);
                return new MarketHistoryDTO
                {
                    ItemID = id,
                    AverageDailySales = result.items.Where(f => f.date > DateTime.UtcNow.AddDays(-31)).Sum(f => f.volume / 30)
                };
            }
            catch (Exception e)
            {
                return new MarketHistoryDTO
                {
                    ItemID = id,
                    Exception = e.Message
                };
            }
        }

        //private static List<MarketOrderDTO> LoadMarketOrders(MarketOrderRequest request, int id)
        //{
        //    var url = string.Format("{3}/market/{0}/orders/{1}/?type=https://api.eveonline.com/types/{2}/",
        //             request.RegionId, request.OrderType.ToString().ToLower(), id, BaseUri);
        //    var readToEnd = JsonLoader.Load(url, request.Token);
        //    var result = JsonConvert.DeserializeObject<MarketOrderResponse>(readToEnd);
        //    var list = result.Items.Select(f => new MarketOrderDTO
        //    {
        //        ItemID = f.type.id,
        //        OrderID = f.id,
        //        OrderType = f.buy ? OrderType.Buy : OrderType.Sell,
        //        Price = f.price,
        //        StationID = f.location.id,
        //        Range = GetRange(f.range)
        //    }).ToList();
        //    return list;
        //}

        private static short GetRange(string range)
        {
            switch (range)
            {
                case "region":
                    return 32767;
                case "station":
                case "solarsystem":
                    return 0;
                default:
                    return short.Parse(range);
            }
        }
    }
}
