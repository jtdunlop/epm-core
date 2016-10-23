using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DBSoft.EPM.DAL.Requests;
using DBSoft.EPM.DAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dbsoft.Epm.Web.Controllers
{
    public class EpmController : Controller
    {
        private readonly IUserService _users;

        protected EpmController(IUserService users)
        {
            _users = users;
        }

        protected bool IsEveClient;
        protected string Token { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.HideMenu = false;
            IsEveClient = HttpContext.Request.Headers["EVE_TRUSTED"] == "Yes";
            if (HttpContext.Session.GetString("Token") == null)
            {
                HttpContext.Session.SetString("Token", Guid.NewGuid().ToString());
            }
            Token = new Guid(HttpContext.Session.GetString("Token")).ToString();
            ViewBag.Token = Token;

            Authenticate("SJ Astralana");
            ViewBag.IsAdmin = _users.IsAdmin(Token);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // If I already know you, I already have your refresh token
                Authenticate(HttpContext.User.Claims.ToList());
                ViewBag.IsAdmin = _users.IsAdmin(Token);
            }
            else
            {
                // ViewBag.IsAdmin = false;
            }
            base.OnActionExecuting(context);
        }

        private void Authenticate(string user)
        {
            var request = new SsoAuthenticateRequest
            {
                EveOnlineCharacter = user,
                SessionToken = Token,
                RefreshToken = null
            };
            _users.Authenticate(request);
        }

        protected void Authenticate(List<Claim> claims)
        {
            var user = claims.Single(f => f.Type == ClaimTypes.Name).Value;
            var refresh = claims.SingleOrDefault(f => f.Type == ClaimTypes.UserData);
            var request = new SsoAuthenticateRequest
            {
                EveOnlineCharacter = user,
                SessionToken = Token,
                RefreshToken = refresh?.Value
            };
            _users.Authenticate(request);
        }
    }
}