namespace DBSoft.EPM.Logic.RefreshApi
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Config;
    using DAL.Annotations;
    using DAL.Enums;
    using DAL.Interfaces;
    using DAL.Requests;
    using DAL.Services;
    using DAL.Services.Materials;
    using EVEAPI.Crest;
    using EVEAPI.Crest.MarketOrder;
    using Microsoft.Framework.OptionsModel;
    using NLog;
    using OrderType = EVEAPI.Entities.MarketOrder.OrderType;

    public interface IMarketImportMapper
    {
        Task Pull(string token);
    }

    [UsedImplicitly]
    public class MarketImportMapper : EveApiMapper, IMarketImportMapper
    {
        private readonly IMarketService _marketService;
        private readonly IMaterialItemService _materials;
        private readonly IItemService _items;
        private readonly IConfigurationService _config;
        private readonly IUniverseService _universe;
        private readonly IMarketImportService _marketImportService;
        private readonly IJumpService _jumps;
        private readonly IOptions<Authentication> _options;
        private readonly IUserAuth _auth;
        private readonly IUserService _users;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MarketImportMapper(IMarketService marketService, IMaterialItemService materials, IItemService items, IConfigurationService config,
            IUniverseService universe, IMarketImportService marketImportService, IEveApiStatusService statusService,
            IJumpService jumps, IOptions<Authentication> options, IUserAuth auth, IUserService users)
            : base(statusService)
        {
            _marketService = marketService;
            _materials = materials;
            _items = items;
            _config = config;
            _universe = universe;
            _marketImportService = marketImportService;
            _jumps = jumps;
            _options = options;
            _auth = auth;
            _users = users;
        }

        public async Task Pull(string token)
        {

            try
            {
                var items = _items.ListBuildable(token).Select(f => f.ItemID).ToList();
                var market = _config.GetSetting<int>(token, ConfigurationType.MarketSellLocation);
                var marketRegion = _universe.GetStationSolarSystem(market)
                    .RegionID;
                var refresh = _users.GetAuthenticatedUser(token).SsoRefreshToken;
                var options = _options.Options.EveSso;
                var ssoToken = (await _auth.RefreshAuthenticatedUser(refresh, options.ClientId, options.ClientSecret)).Token;
                var orders = _marketService.ListOrders(new MarketOrderRequest
                {
                    ItemIds = items,
                    Token = ssoToken,
                    OrderType = OrderType.Sell,
                    RegionId = marketRegion
                });

                var materials = _materials.ListBuildable(token).Select(f => f.ItemId).ToList();


                var buy = _config.GetSetting<int>(token, ConfigurationType.MarketBuyLocation);
                if (buy == 0)
                {
                    buy = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);

                }

                var buySystem = _universe.GetStationSolarSystem(buy).SolarSystemID;
                var buyRegion = _universe.GetStationSolarSystem(buy).RegionID;

                // We need buy and sell prices for materials because overcut is a % of the difference between the two.
                var result = _marketService.ListOrders(new MarketOrderRequest
                {
                    ItemIds = materials,
                    Token = ssoToken,
                    OrderType = OrderType.Buy,
                    RegionId = buyRegion
                });
                orders.Orders.AddRange(result.Orders);
                foreach ( var citadel in result.Citadels)
                {
                    _universe.AddCitadel(citadel);
                }
                result = _marketService.ListOrders(new MarketOrderRequest
                {
                    ItemIds = materials,
                    Token = ssoToken,
                    OrderType = OrderType.Sell,
                    RegionId = buyRegion
                });
                orders.Orders.AddRange(result.Orders);
                foreach (var citadel in result.Citadels)
                {
                    _universe.AddCitadel(citadel);
                }

                var grouped = from o in orders.Orders
                              group o by new { o.ItemID, o.StationID, o.OrderType, o.Range } into grp
                              select new
                              {
                                  grp.Key.ItemID,
                                  grp.Key.StationID,
                                  grp.Key.OrderType,
                                  grp.Key.Range,
                                  MaxPrice = grp.Max(f => f.Price),
                                  MinPrice = grp.Min(f => f.Price)
                              };


                var list = grouped.Select(order => new SaveMarketImportRequest
                {
                    ItemID = order.ItemID,
                    OrderType = (DAL.CodeFirst.Models.OrderType)order.OrderType,
                    Price = (decimal)(order.OrderType == OrderType.Buy ? order.MaxPrice : order.MinPrice),
                    RegionID = _universe.GetStationSolarSystem(order.StationID).RegionID,
                    SolarSystemID = _universe.GetStationSolarSystem(order.StationID).SolarSystemID,
                    StationID = order.StationID,
                    Jumps = (short)(order.OrderType == OrderType.Buy ? _jumps.GetJumps(buySystem, _universe.GetStationSolarSystem(order.StationID).SolarSystemID) : 0),
                    Range = order.Range
                }).ToList();
                await _marketImportService.SaveMarketImports(token, DateTime.UtcNow, list);
                UpdateStatus("MarketImports", DateTime.UtcNow.AddMinutes(5), token);
            }
            catch (AggregateException e)
            {
                SaveError("MarketImports", token, e.InnerExceptions.First().Message);
                Logger.Error("MarketImport {0}", e.ToString());
            }
            catch (Exception e)
            {
                SaveError("MarketImports", token, e.Message);
                Logger.Error("MarketImport {0}", e.ToString());
            }

        }
    }
}
