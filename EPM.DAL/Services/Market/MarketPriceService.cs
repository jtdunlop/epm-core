
namespace DBSoft.EPM.DAL.Services.Market
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using Aspects;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Enums;
    using Interfaces;
    using Queries;
    using Requests;

    public class MarketPriceService : IMarketPriceService
    {
        private readonly DbContext _context;
        private OrderType _orderType;
        private readonly IConfigurationService _config;
        private readonly IUniverseService _universe;
        private readonly IUserService _users;

        public MarketPriceService(IDbContextFactory factory, IConfigurationService config, IUniverseService universe, IUserService users)
        {
            _context = factory.CreateContext();
            _config = config;
            _universe = universe;
            _users = users;
        }

        [Trace, Cache.Cacheable]
        public List<MarketPriceDTO> List(MarketPriceRequest request)
        {
            ValidateToken(request.Token);

            if (request.OrderType.HasValue)
            {
                _orderType = request.OrderType.Value;
            }

            var dateRange = request.DateRange ?? new DateRange { StartDate = DateTime.UtcNow.AddDays(-2), EndDate = DateTime.UtcNow };
            var prices = request.Range.HasValue ?
                MostRecentRangePrices(request.Range.Value, dateRange, request.Token) :
                MostRecentHubPrices(dateRange, request.Token);

            var result = from price in prices
                         select new MarketPriceDTO
                         {
                             ItemID = price.ItemID,
                             ItemName = price.Item.Name,
                             CurrentPrice = price.Price,
                             Timestamp = price.TimeStamp
                         };

            return result.ToList();
        }

        private static void ValidateToken(string token)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(token));
        }

        private List<MarketImport2> MostRecentRangePrices(int jumps, DateRange dateRange, string token)
        {
            var buyID = _config.GetSetting<int>(token, ConfigurationType.MarketBuyLocation);
            if ( buyID == 0 )
            {
                buyID = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
            }
            var regionID = _universe.GetStationSolarSystem(buyID).RegionID;

            
            var userID = _users.GetUserID(token);

            Expression<Func<MarketImport2, bool>> spec = f => f.Range >= jumps &&
                f.Jumps <= jumps && f.RegionID == regionID && f.OrderType == _orderType;
            return MostRecentPrices(spec, dateRange, userID);
        }

        private List<MarketImport2> MostRecentHubPrices(DateRange dateRange, string token)
        {
            var buyID = _config.GetSetting<int>(token, ConfigurationType.MarketSellLocation);
           
            var userID = _users.GetUserID(token);

            Expression<Func<MarketImport2, bool>> spec = f => f.StationID == buyID && f.OrderType == _orderType;
            return MostRecentPrices(spec, dateRange, userID);
        }

        private List<MarketImport2> MostRecentPrices(Expression<Func<MarketImport2, bool>> spec, DateRange dateRange, int userID)
        {
            var grouped = new DataQuery<MarketImport2>(_context)
                .Specify(spec)
                .Specify(f => f.UserID == userID)
                .Specify(f => f.TimeStamp >= dateRange.StartDate && f.TimeStamp <= dateRange.EndDate)
                .GetQuery()
                .GroupBy(g => g.ItemID)
                // Returning an iqueryable doesn't generate code that orders the results properly and the wrong price is selected
                .ToList();

            Func<IGrouping<int, MarketImport2>, IOrderedEnumerable<MarketImport2>> buySelector =
                f => f.OrderByDescending(o => o.TimeStamp).ThenByDescending(p => p.Price);
            Func<IGrouping<int, MarketImport2>, IOrderedEnumerable<MarketImport2>> sellSelector =
                s => s.OrderByDescending(o => o.TimeStamp).ThenBy(p => p.Price);

            var selector = _orderType == OrderType.Buy ? buySelector : sellSelector;
            var result = grouped.Select(selector).Select(s => s.FirstOrDefault()).ToList();
            return result;
        }
    }
}

