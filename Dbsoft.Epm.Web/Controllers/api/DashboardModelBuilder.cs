namespace Dbsoft.Epm.Web.Controllers.api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DBSoft.EPM.DAL;
    using DBSoft.EPM.DAL.CodeFirst.Models;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Interfaces;
    using DBSoft.EPM.DAL.Services;
    using DBSoft.EPM.DAL.Services.Transactions;

    public class DashboardModelBuilder
    {
        private readonly IAccountBalanceService _accountBalanceService;
        private readonly IItemTransactionService _itemTransactionService;
        private readonly IAssetCapitalService _assetCapitalService;

        public DashboardModelBuilder(IAccountBalanceService accountBalanceService, IItemTransactionService itemTransactionService, IAssetCapitalService assetCapitalService)
        {
            _accountBalanceService = accountBalanceService;
            _itemTransactionService = itemTransactionService;
            _assetCapitalService = assetCapitalService;
        }

        public DashboardModel CreateModel(string token)
        {
            var items = _assetCapitalService.ListItemCapital(token).Sum(f => f.TotalValue);
            var materials =
                _assetCapitalService.ListMaterialCapital(token).Sum(f => f.FactoryValue + f.MarketValue + f.RemoteValue);
            var tran7 = GetTransactions(token, 7).ToList();
            var tran30 = GetTransactions(token, 30).ToList();
            var model = new DashboardModel
            {
                Capital = items + materials,
                Sales30 = new SalesModel
                {
                    Profit = tran30.Sum(f => f.GpAmt),
                    Sales = tran30.Sum(f => f.GrossAmount)
                },
                Sales7 = new SalesModel
                {
                    Profit = tran7.Sum(f => f.GpAmt),
                    Sales = tran7.Sum(f => f.GrossAmount)
                },
                WalletBalance = _accountBalanceService.List(token)
                    .Where(f => f.AccountKey == AccountBalanceService.MasterWallet)
                    .Sum(f => f.Balance)
            };
            return model;
        }

        private IEnumerable<ItemTransactionByItemDto> GetTransactions(string token, int numDays)
        {
            return _itemTransactionService.ListByItem(new ItemTransactionRequest
            {
                Token = token,
                TransactionType = TransactionType.Sell,
                DateRange = new DateRange { StartDate = DateTime.UtcNow.AddDays(-numDays), EndDate = DateTime.UtcNow.AddDays(-1) }
            });
        }
    }
}