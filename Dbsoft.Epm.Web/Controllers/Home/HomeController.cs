using System.Collections.Generic;
using System.Linq;
using DBSoft.EPM.DAL.Services;
using DBSoft.EPM.DAL.Services.AccountApi;
using DBSoft.EVEAPI.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dbsoft.Epm.Web.Controllers.Home
{
    public class HomeController : EpmController
    {
        private readonly IAccountApiService _accounts;
        private readonly IUserService _users;

        public HomeController(IAccountApiService accounts, IUserService users) : base(users)
        {
            _accounts = accounts;
            _users = users;
        }

        public IActionResult Index(string impersonate)
        {
            ViewBag.HideMenu = false;
            if (!string.IsNullOrEmpty(impersonate))
            {
                _users.Impersonate(impersonate, Token);
            }
            var accounts = _accounts.List(Token);

            var model = new DashboardModel
            {
                DashboardImages = GetDashboardImages(accounts),
                HelpUrl = "https://dbsoft.atlassian.net/wiki/display/EPM/Dashboard",
                IsAdmin = _users.IsAdmin(Token)
            };
            return View(model);
        }

        private static IEnumerable<DashboardImage> GetDashboardImages(IEnumerable<AccountApiDTO> accounts)
        {
            return accounts.Select(account => new DashboardImage
            {
                CharacterName = account.AccountName,
                ImageUrl = string.Format("http://image.eveonline.com/{1}/{0}_128.{2}",
                    account.EveApiID,
                    account.ApiKeyType == ApiKeyType.Character ? "Character" : "Corporation",
                    account.ApiKeyType == ApiKeyType.Character ? "jpg" : "png")
            }).ToList();
        }
    }
}
