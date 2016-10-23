namespace DBSoft.EPM.DAL.Services.Market
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Aspects;
    using AutoMapper;
    using CodeFirst.Models;
    using Commands;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Interfaces;
    using Queries;
    using Requests;

    public class MarketOrderService : IMarketOrderService
	{
        private readonly IDbContextFactory _factory;
        private readonly IUserService _users;

        public MarketOrderService(IDbContextFactory factory, IUserService users)
		{
		    _factory = factory;
            _users = users;
		}

        [Trace,Cache.Cacheable]
		public List<MarketOrderDTO> List(MarketOrderRequest request)
		{
            var token = request.Token;
			ValidateToken(token);

            var context = _factory.CreateContext();
            var orders = new DataQuery<MarketOrder>(context);
            if ( request.RegionId.HasValue )
            {
                orders.Specify(f => f.Station.SolarSystem.RegionID == request.RegionId.Value);
            }

            var userID = _users.GetUserID(token);
			var result = from order in orders.Specify(f => f.UserID == userID && f.OrderStatus == OrderStatus.Active).GetQuery()
						 select new MarketOrderDTO
						 {
							 ItemID = order.ItemID,
							 ItemName = order.Item.Name,
							 RemainingQuantity = order.RemainingQuantity,
							 Price = order.Price,
							 Escrow = order.Escrow
						 };
			return result.ToList();
		}

		private static void ValidateToken(string token)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(token));
		}

		public void SaveOrder(SaveMarketOrderRequest request)
		{
			ValidateToken(request.Token);

            var context = _factory.CreateContext();
            using (var cmd = new DataCommand(context))
			{
				var userID = _users.GetUserID(request.Token);
				var order = cmd.Get<MarketOrder>(f => f.EveMarketOrderID == request.OrderID && f.UserID == userID );
				Mapper.Map(request, order);
				order.UserID = userID;
				context.SaveChanges();
			}
		}

		public async Task DeleteAll(string token)
		{
            var context = _factory.CreateContext();
			var userID = _users.GetUserID(token);
			foreach (var order in context.Set<MarketOrder>().Where(f => f.UserID == userID))
			{
				context.Set<MarketOrder>().Remove(order);
			}
			await context.SaveChangesAsync();
		}
	}

    public class MarketOrderRequest
    {
        public string Token { get; set; }
        public int? RegionId { get; set; }
    }
}
