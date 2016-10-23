using DBSoft.EPM.DAL.Interfaces;
using DBSoft.EPM.DAL.Services.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Dbsoft.Epm.Web.Controllers.api
{
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private readonly DashboardModelBuilder _dashboardModelBuilder;

        public DashboardController(IAccountBalanceService accountBalanceService, IItemTransactionService itemTransactionService,
            IAssetCapitalService assetCapitalService)
        {
            _dashboardModelBuilder = new DashboardModelBuilder(accountBalanceService, itemTransactionService, assetCapitalService);
        }

        public DashboardModel Get(string token)
        {
            var model = _dashboardModelBuilder.CreateModel(token);
            return model;
        }
    }
}
