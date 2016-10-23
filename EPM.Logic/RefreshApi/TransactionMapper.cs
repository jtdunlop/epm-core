namespace DBSoft.EPM.Logic.RefreshApi
{
    using System.Threading.Tasks;
    using DAL.DTOs;
    using DAL.Services.AccountApi;
    using DAL.CodeFirst.Models;
	using DAL.Interfaces;
    using DAL.Services.ItemCosts;
    using DAL.Services.Transactions;
    using EVEAPI.Entities.WalletTransaction;
	using System;
	using System.Collections.Generic;
	using System.Linq;
    using DAL.Aspects;
    using JetBrains.Annotations;

    public interface ITransactionMapper
    {
        Task Pull(string token);
    }

    [UsedImplicitly]
    public class TransactionMapper : EveApiMapper, ITransactionMapper
    {
		private readonly ITransactionService _transactionService;
        private readonly IItemService _items;
        private readonly IItemCostService _itemCostService;
	    private readonly IAccountApiService _accounts;
		private readonly IWalletTransactionService _service;

		public TransactionMapper(IWalletTransactionService service, 
            IEveApiStatusService statusService, IItemCostService costs, IAccountApiService accounts,
            ITransactionService transactionService, IItemService items) : base(statusService)
		{
			_service = service;
            _transactionService = transactionService;
		    _items = items;
		    _itemCostService = costs;
		    _accounts = accounts;
		}

		[Trace]
		public async Task Pull(string token)
		{
			var last = _transactionService.GetLastTransactionID(token);
			const string serviceName = "WalletTransactions";
           

			var cachedUntil = DateTime.Now;
            foreach ( var account in _accounts.List(token))
            {
                try
                {
                    // This will always pull the 1000 most recent transactions, duplicates will be ignored. 
                    // Need to do this to support mocking.
                    var response = await _service.Load(account.ApiKeyType, account.ApiKeyID, account.ApiVerificationCode, account.EveApiID, 30, last);
                    await ProcessTransactions(response.Data, token);
                    cachedUntil = response.CachedUntil;
                }
                catch (Exception e)
                {
                    SaveError(serviceName, token, e.Message);
                    throw;
                }
            }
			UpdateStatus(serviceName, cachedUntil, token);
		}

		[Trace]
		private async Task ProcessTransactions(List<WalletTransaction> eveTransactions, string token)
		{
			var listable = _items.ListBuildable(token);
			if (!listable.Any())
			{
				throw new MapperException("No configured blueprints found");
			}
			// Put purchases in first to update costs
			var requests = BuildRequests(eveTransactions, TransactionType.Buy, token);
			await _transactionService.SaveTransactions(token, requests);
			requests = BuildRequests(eveTransactions, TransactionType.Sell, token);
			await _transactionService.SaveTransactions(token, requests);
		}

		private List<SaveTransactionRequest> BuildRequests(IEnumerable<WalletTransaction> eveTransactions, 
            TransactionType transactionType, string token)
		{
			IEnumerable<ItemCostDTO> costs = new List<ItemCostDTO>();
			if (transactionType == TransactionType.Sell)
			{
				costs = _itemCostService.ListBuildable(new ListBuildableRequest { Token = token });
			}
			var buildable = _items.ListBuildable(token);

			var tt = transactionType == TransactionType.Buy ? "buy" : "sell";
			return (from transaction in eveTransactions
					where transaction.Type == tt
					select new SaveTransactionRequest
					{
						EveTransactionID = transaction.ID,
						ItemID = transaction.TypeID,
						Quantity = transaction.Quantity,
						Price = transaction.Price,
						TransactionType = ConvertTransactionType(transaction.Type),
						DateTime = transaction.DateTime,
						Cost = transactionType == TransactionType.Buy ? null : GetCost(transaction.TypeID, buildable, costs),
						VisibleFlag = transactionType == TransactionType.Buy || buildable.Any(f => f.ItemID == transaction.TypeID)
					}).ToList();
		}

		private static decimal? GetCost(int itemId, IEnumerable<BuildableItemDTO> items, IEnumerable<ItemCostDTO> costs)
		{
			var itemcost = costs.SingleOrDefault(f => f.ItemID == itemId);
		    if (itemcost != null) return itemcost.Cost;
            if ( itemcost == null && items.Any(f => f.ItemID == itemId))
            {
                throw new MapperException($"Cost for item {itemId} not found"); 
            }
		    return null;
		}

		private static TransactionType ConvertTransactionType(string type)
		{
			return type == "buy" ? TransactionType.Buy : TransactionType.Sell;
		}
	}
}
