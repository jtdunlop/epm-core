namespace DBSoft.EPM.Logic.RefreshApi
{
    using System.Threading.Tasks;
    using DAL.Annotations;
    using DAL.Services.AccountApi;
    using System;
	using System.Data.Entity.Core;
	using System.Data.Entity.Infrastructure;
	using System.Linq;
	using AutoMapper;
	using DAL.Interfaces;
	using DAL.Requests;
	using System.Collections.Generic;
	using NLog;
    using IMarketOrderService = DAL.Interfaces.IMarketOrderService;
    using MarketOrder = EVEAPI.Entities.MarketOrder.MarketOrder;

    [UsedImplicitly]
    public class MarketOrderMapper : EveApiMapper, IMarketOrderMapper
    {
		private readonly IMarketOrderService _marketOrderService;
        private readonly IAccountApiService _accounts;
        private readonly EVEAPI.Entities.MarketOrder.IMarketOrderService _orders;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public MarketOrderMapper(IEveApiStatusService statusService, 
            IMarketOrderService marketOrderService, IAccountApiService accounts, EVEAPI.Entities.MarketOrder.IMarketOrderService orders) : base(statusService)
		{
			_marketOrderService = marketOrderService;
		    _accounts = accounts;
		    _orders = orders;
		}

		public async Task Pull(string token)
		{
			await _marketOrderService.DeleteAll(token);

			const string serviceName = "MarketOrders";
            
			var cachedUntil = DateTime.Now;

			foreach ( var account in _accounts.List(token) )
            {
                try
                {
                    var result = await _orders.Load(account.ApiKeyType, account.ApiKeyID, account.ApiVerificationCode, account.EveApiID);
                    ProcessOrders(token, result.Data);
                    cachedUntil = result.CachedUntil;
                }
                catch (Exception e)
                {
                    SaveError(serviceName, token, e.Message);
                    throw;
                }
            }
			UpdateStatus(serviceName, cachedUntil,token);
		}

		private void ProcessOrders(string token, IEnumerable<MarketOrder> eveOrders)
		{
			foreach (var request in eveOrders.Select(Mapper.Map<SaveMarketOrderRequest>))
			{
				request.Token = token;
				try
				{
					_marketOrderService.SaveOrder(request);
				}
				catch (DbUpdateException e)
				{
					if (e.InnerException is UpdateException)
					{
						Logger.Warn("Unable to save market order");
					}
				}
			}
		}
	}
}
