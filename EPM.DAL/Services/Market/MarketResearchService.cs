namespace DBSoft.EPM.DAL.Services.Market
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Annotations;
    using Aspects;
    using AutoMapper;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Enums;
    using EVEAPI.Crest.MarketOrder;
    using Interfaces;
    using ItemCosts;
    using Materials;
    using NLog;
    using MarketHistoryRequest = EVEAPI.Crest.MarketOrder.MarketHistoryRequest;
    using OrderType = EVEAPI.Entities.MarketOrder.OrderType;

    [UsedImplicitly]
    public class MarketResearchService : IMarketResearchService
    {
        private readonly IMarketService _market;
        private readonly IItemService _items;
        private readonly IBuildMaterialService _materials;
        private readonly IUniverseService _universe;
        private readonly IConfigurationService _config;
        private readonly IBlueprintService _blueprints;
        private readonly IBlueprintInstanceService _instances;
        private readonly DbContext _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MarketResearchService(IMarketService market, IItemService items, IBuildMaterialService materials,
            IUniverseService universe, IConfigurationService config, IBlueprintService blueprints, IDbContextFactory factory,
            IBlueprintInstanceService instances)
        {
            _market = market;
            _items = items;
            _materials = materials;
            _universe = universe;
            _config = config;
            _blueprints = blueprints;
            _instances = instances;
            _context = factory.CreateContext();
        }

        [Cache.Cacheable, Trace]
        public List<MarketResearchDTO> List(string token)
        {
            var station = _config.GetSetting<int>(token, ConfigurationType.MarketSellLocation);
            var factory = _universe.GetStationSolarSystem(_config.GetSetting<int>(token, ConfigurationType.FactoryLocation));
            var research = _context.Set<MarketResearch>().Where(f => f.StationID == station).ToList();
            var instances = _instances.ListDistinctOwned(token);
            var result = from r in research
                         from instance in instances.Where(f => f.BuildItemID == r.ItemID).DefaultIfEmpty()
                         select new MarketResearchDTO
                         {
                             Cost = r.Cost + ItemCostService.GetInstallationCost(r.Cost, factory),
                             Inventory = r.Inventory,
                             ItemID = r.ItemID,
                             ItemName = r.ItemName,
                             Price = r.Price,
                             ProductionTime = r.ProductionTime,
                             Volume = r.Volume,
                             Competitors = r.Competitors,
                             QuantityMultiplier = r.QuantityMultiplier,
                             IsMine = instance != null
                         };
            return result.ToList();
        }

        public async Task Update(int stationId)
        {
            var items = _items.ListProducible();
            Logger.Trace("Items: {0}", items.Count);
            var marketData = await LoadMarketData(stationId);
            // Logger.Trace("MarketData: {0}", marketData.Count);
            var marketHistory = LoadMarketHistory(stationId);
            //Logger.Trace("MarketHistory: {0}", marketHistory.Count);
            var costs = GetCosts(items, marketData);
            //Logger.Trace("Costs: {0}", costs.ToList().Count);
            var prices = GetPrices(marketData);
            //Logger.Trace("Prices: {0}", prices.ToList().Count);
            var volumes = GetVolumes(marketData).Where(f => f.Volume > 0);
            //Logger.Trace("Volumes: {0}", volumes.ToList().Count);
            var c = costs.Where(f => f.BaseCost > 0);
            //Logger.Trace("Items: {0}", items.Count);
            var blueprints = _blueprints.List();
            //Logger.Trace("Blueprints: {0}", blueprints.Count);

            var result = from item in items
                         join cost in c on item.ItemID equals cost.ItemID
                         join price in prices on item.ItemID equals price.ItemID
                         join blueprint in blueprints on item.ItemID equals blueprint.BuildItemID
                         join volume in volumes on item.ItemID equals volume.ItemID
                         join history in marketHistory on item.ItemID equals history.ItemID
                         select new MarketResearchDTO
                         {
                             ItemID = item.ItemID,
                             ItemName = item.ItemName,
                             Price = price.CurrentPrice,
                             Cost = cost.Cost,
                             ProductionTime = TimeSpan.FromSeconds(blueprint.ProductionTime / 1.1),
                             QuantityMultiplier = item.QuantityMultiplier,
                             Inventory = volume.Volume,
                             Volume = history.AverageDailySales,
                             Competitors = volume.Competitors
                         };

            Save(result.Where(f => f.ProductionTime < TimeSpan.FromDays(1)).ToList(), stationId);
        }

        private void Save(IEnumerable<MarketResearchDTO> results, int stationId)
        {
            foreach ( var item in _context.Set<MarketResearch>().Where(f => f.StationID == stationId) )
            {
                _context.Set<MarketResearch>().Remove(item);
            }
            _context.Set<MarketResearch>().AddRange(results.Select(f => new MarketResearch
            {
                Cost = f.Cost,
                Inventory = f.Inventory,
                ItemID = f.ItemID,
                ItemName = f.ItemName,
                Price = f.Price,
                ProductionTime = f.ProductionTime,
                StationID = stationId,
                Volume = f.Volume,
                Competitors = f.Competitors,
                QuantityMultiplier = f.QuantityMultiplier
            }));
            _context.SaveChanges();
        }

        private static IEnumerable<ItemVolumeDTO> GetVolumes(IEnumerable<MarketSummaryItem> marketData)
        {
            var volumes = marketData.Where(f => f.MinimumSellPrice > 0).Select(f => new ItemVolumeDTO
            {
                ItemID = f.ItemID,
                Volume = f.SellVolume,
                Competitors = f.Competitors
            }).ToList();
            Logger.Trace("Volumes: {0}", volumes.Count);
            return volumes;
        }

        [Trace]
        private IEnumerable<ItemCostDTO> GetCosts(IEnumerable<ItemDTO> items, IEnumerable<MarketSummaryItem> marketData)
        {
            var materials = _materials.ListProducible(null);

            var use = items
                .Where(f => materials.Where(g => g.ItemId == f.ItemID)
                    .All(h => marketData.Any(i => i.ItemID == h.MaterialId)));

            var result =
                (from material in materials
                join item in use on material.ItemId equals item.ItemID
                from price in marketData.Where(f => f.ItemID == material.MaterialId)
                orderby item.ItemID
                group new { b = item, price, material } by
                    new { item.ItemID, item.ItemName, item.QuantityMultiplier}
                    into grp
                    select new ItemCostDTO(_config)
                    {
                        ItemID = grp.Key.ItemID,
                        ItemName = grp.Key.ItemName,
                        MaterialEfficiency = 10,
                        BaseCost = grp.Sum(f => (f.material.Quantity + f.material.AdditionalQuantity) *
                            f.price.MaximumBuyPrice.GetValueOrDefault() /
                            grp.Key.QuantityMultiplier)
                    }).ToList();

            Logger.Trace("Costs: {0}", result.Count);
            return result;
        }

        private static IEnumerable<MarketPriceDTO> GetPrices(IEnumerable<MarketSummaryItem> marketData)
        {
            var result = marketData.Where(f => f.MinimumSellPrice > 0).Select(f => new MarketPriceDTO
            {
                CurrentPrice = f.MinimumSellPrice,
                ItemID = f.ItemID
            }).ToList();
            Logger.Trace("Prices: {0}", result.Count);
            return result;
        }

        [Trace]
        private async Task<List<MarketSummaryItem>> LoadMarketData(int stationId)
        {
            var items = _items.ListProducible();
            var station = _universe.GetStation(stationId);
            var solarSystem = _universe.ListSolarSystems().Single(f => f.SolarSystemID == station.SolarSystemID);
            var materials = _items.ListProducibleMaterials();

            var request = new MarketSummaryRequest
            {
                StationID = stationId,
                ItemIDs = items.Select(f => f.ItemID).ToList(),
                RegionId = solarSystem.RegionID,
                OrderType = OrderType.Sell
            };
            // var t1 = new List<MarketSummaryDTO>();
            var ms = await _market.ListSummaries(request);
            Logger.Debug($"Summaries: {ms.Count}");
            var errors = ms.Where(f => f.Exception != null).ToList();
            var t1 = ms.Where(f => f.SellVolume > 0 && f.MinimumSellPrice > 0).ToList();

            request = new MarketSummaryRequest
            {
                StationID = stationId,
                ItemIDs = materials.Select(f => f.ItemID).ToList(),
                RegionId = solarSystem.RegionID,
                OrderType = OrderType.Buy
            };
            var more = await _market.ListSummaries(request);
            Logger.Debug($"Summaries: {more.Count}");
            var other = more.Where(f => f.SellVolume > 0 && f.MaximumBuyPrice > 0);
            t1.AddRange(other);
            errors.AddRange(more.Where(f => f.Exception != null));
            var distinct = from error in errors
                           group error by error.Exception into grp
                           select new { Error = grp.Key, Count = grp.Count() };
            Logger.Trace("LoadMarketData: {0}", t1.Count);
            foreach (var d in distinct)
            {
                Logger.Trace("{0}: {1}", d.Error, d.Count);
            }
            
            return t1.Select(Mapper.Map<MarketSummaryItem>).ToList();
        }

        [Trace]
        private List<MarketHistoryItem> LoadMarketHistory(int stationId)
        {
            var items = _items.ListProducible();
            var region = _universe.GetStationSolarSystem(stationId).RegionID;
            var request = new MarketHistoryRequest
            {
                RegionID = region,
                ItemIds = items.Select(f => f.ItemID).ToList()
            };
            var history = _market.ListHistory(request);
            var result = history.Where(f => string.IsNullOrWhiteSpace(f.Exception)).ToList();
            Logger.Trace("LoadMarketHistory: {0}", result.Count);
            return result.Select(f => new MarketHistoryItem { ItemID = f.ItemID, AverageDailySales = f.AverageDailySales }).ToList();
        }

    }
}
