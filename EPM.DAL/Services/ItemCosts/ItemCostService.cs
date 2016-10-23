namespace DBSoft.EPM.DAL.Services.ItemCosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Annotations;
    using Aspects;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Enums;
    using Interfaces;
    using Market;
    using MaterialCosts;
    using Materials;
    using Requests;

    [UsedImplicitly]
    public class ItemCostService : IItemCostService
    {
        private readonly IMaterialCostService _costs;
        private readonly IItemService _items;
        private readonly IConfigurationService _config;
        private readonly IBuildMaterialService _materials;
        private readonly IUniverseService _universe;
        private readonly IMarketPriceService _prices;
        private readonly IFreightCalculator _calculator;

        public ItemCostService(IMaterialCostService costs, IItemService items, IConfigurationService config,
            IBuildMaterialService materials,
            IUniverseService universe, IMarketPriceService prices, IFreightCalculator calculator)
        {
            _costs = costs;
            _items = items;
            _config = config;
            _materials = materials;
            _universe = universe;
            _prices = prices;
            _calculator = calculator;
        }

        [Trace, Cache.Cacheable]
        public List<ItemCostDTO> ListBuildable(ListBuildableRequest request)
        {
            var items = _items.ListBuildable(request.Token);
            var result = GetItemCosts(request.Token, request.Date, items);
            return result;
        }

        [Trace, Cache.Cacheable]
        public IEnumerable<ItemCostVarianceDto> ListVariances(string token)
        {
            var costNow = ListBuildable(new ListBuildableRequest
            {
                Token = token,
                Date = DateTime.Now.StartOfTheDay()
            });
            var costThen = ListBuildable(new ListBuildableRequest
            {
                Token = token,
                Date = DateTime.Now.AddDays(-7).StartOfTheDay()
            });

            var result = from now in costNow
                         join then in costThen on now.ItemID equals then.ItemID
                         select new ItemCostVarianceDto
                         {
                             ItemId = now.ItemID,
                             ItemName = now.ItemName,
                             CostNow = now.BaseCost,
                             CostThen = then.BaseCost
                         };
            return result;
        }

        [Trace, Cache.Cacheable]
        public IEnumerable<MaterialCostVarianceByItemDto> ListVariancesByItem(string token)
        {
            var variances = _costs.ListVariances(token);
            var materials = _materials.ListBuildable(token);
            var items = ListBuildable(new ListBuildableRequest
            {
                Token = token
            });


            var result = from variance in variances
                         join material in materials on variance.MaterialId equals material.MaterialId
                         join item in items on material.ItemId equals item.ItemID
                         select new MaterialCostVarianceByItemDto
                         {
                             ItemId = material.ItemId,
                             ItemName = material.ItemName,
                             CostNow = variance.CostNow,
                             CostThen = variance.CostThen,
                             MaterialId = variance.MaterialId,
                             MaterialName = variance.MaterialName,
                             MaterialQuantity = material.Quantity / material.QuantityMultiplier,
                             ItemCost = item.Cost
                         };
            return result;

        }


        public static decimal GetInstallationCost(decimal cost, SolarSystemDTO system)
        {
            cost *= (1 + system.InstallationTax);
            cost /= .9M;
            return system.ManufacturingCost * cost;
        }

        [Trace]
        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private List<ItemCostDTO> GetItemCosts(string token, DateTime? date, IEnumerable<BuildableItemDTO> items)
        {
            var costs = _costs.List(new MaterialCostRequest
            {
                Token = token,
                Date = date
            }).ToList();
            var materials = _materials.ListBuildable(token).ToList();
            var stationId = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
            var station = _universe.GetStation(stationId);
            var solarSystem = _universe.ListSolarSystems().Single(f => f.SolarSystemID == station.SolarSystemID);
            var tax = station.StationTax;
            var adjustedMarketPrices = _universe.GetAdjustedMarketPrices();
            var prices = _prices.List(new MarketPriceRequest { OrderType = OrderType.Sell, Token = token });


            var mResult =
                from material in materials
                join item in items on material.ItemId equals item.ItemID
                from price in prices.Where(f => f.ItemID == item.ItemID).DefaultIfEmpty()
                join cost in costs on material.MaterialId equals cost.MaterialID
                join adjustedMaterialPrice in adjustedMarketPrices on material.MaterialId equals adjustedMaterialPrice.ItemID
                join adjustedItemPrice in adjustedMarketPrices on material.ItemId equals adjustedItemPrice.ItemID
                orderby item.ItemID
                group new { item, cost, material, adjustedPrice = adjustedMaterialPrice, price } by
                    new
                    {
                        item.ItemID,
                        item.ItemName,
                        item.MaterialEfficiency,
                        item.QuantityMultiplier,
                        item.PerJobAdditionalCost,
                        item.Volume,
                        material.Quantity,
                        adjustedMaterialPrice.AdjustedPrice, 
                        // If no price available fall back to CCP's magic adjusted price
                        // It's only used for freight calculation so no harm
                        CurrentPrice = price?.CurrentPrice ?? adjustedItemPrice.AdjustedPrice
                    }
                    into grp
                    select new
                        {
                            grp.Key.ItemID,
                            grp.Key.ItemName,
                            grp.Key.MaterialEfficiency,
                            BaseCost = grp.Sum(f => f.material.Quantity * f.cost.BaseCost / grp.Key.QuantityMultiplier),
                            FreightCost = grp.Sum(f => f.material.Quantity * f.cost.FreightCost / grp.Key.QuantityMultiplier),
                            AdditionalCost = (grp.Sum(f => 
                                GetInstallationCost(grp.Key.Quantity, solarSystem.ManufacturingCost, grp.Key.AdjustedPrice, tax) + 
                                grp.Key.PerJobAdditionalCost.GetValueOrDefault())) 
                                / grp.Key.QuantityMultiplier,
                            Price = grp.Key.CurrentPrice,
                            grp.Key.Volume
                        };
            var result = from r in mResult
                         group new { r } by
                         new
                         {
                             r.ItemID,
                             r.ItemName,
                             r.MaterialEfficiency,
                             r.Price,
                             r.Volume
                         }
                             into grp
                             select new ItemCostDTO(_config, token)
                                 {
                                     ItemID = grp.Key.ItemID,
                                     ItemName = grp.Key.ItemName,
                                     MaterialEfficiency = grp.Key.MaterialEfficiency,
                                     BaseCost = grp.Sum(f => f.r.BaseCost),
                                     AdditionalCost = grp.Sum(f => f.r.AdditionalCost),
                                     FreightCost = grp.Sum(f => f.r.FreightCost) + _calculator.GetFreightCost(token, grp.Key.Price, grp.Key.Volume)
                                 };
            return result.ToList();
        }

        private static decimal GetInstallationCost(long quantity, decimal manufacturingCost, decimal adjustedPrice, decimal tax)
        {
            var total = adjustedPrice * quantity;
            total *= (1 + tax);
            total /= .9M;
            return manufacturingCost * total;
        }


    }
}
