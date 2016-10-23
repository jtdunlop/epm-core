using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dbsoft.Epm.Web.Controllers.Account
{
    using System.Linq;
    using System.Threading.Tasks;
    using DbSoft.Cache.Aspect;
    using DBSoft.EPM.DAL.Services;
    using Infrastructure;

    public class AccountController : EpmController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _users;

        public AccountController(SignInManager<ApplicationUser> signInManager, IUserService users) : base(users)
        {
            _signInManager = signInManager;
            _users = users;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if ( HttpContext.User.Identity.IsAuthenticated )
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.HideMenu = true;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalCallback", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            // If I don't already know you have to auth here to save a potentially new refresh token
            Authenticate(info.Principal.Claims.ToList());
            return !result.Succeeded ? Login() : RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Logout(string errorMessage)
        {
            await _signInManager.SignOutAsync();
            _users.Logout(Token);
            CacheService.ClearCache(Token);
            return RedirectToAction("Login", new { errorMessage });
        }
    }


}
