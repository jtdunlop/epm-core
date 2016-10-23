using DBSoft.EPM.DAL;
using DBSoft.EPM.DAL.Factories;
using DBSoft.EPM.DAL.Services;
using DBSoft.EPM.DAL.Services.AccountApi;
using DBSoft.EPM.Logic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dbsoft.Epm.Web.Infrastructure.Filters
{
    public class RequireCompleteConfigurationAttribute : ActionFilterAttribute 
	{
		private readonly IConfigurationProcessor _configProcessor;

		public RequireCompleteConfigurationAttribute()
		{
			//var factory = new EPMEntitiesFactory(new ConnectionStringProvider(new EpmConfig()));
   //         var users = new UserService(factory);
			//_configProcessor = new ConfigurationProcessor(new ConfigurationService(factory, users), new AccountApiService(factory));
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			//var token = AuthenticationHelper.GetAuthenticationTicket(filterContext.HttpContext);
			//if (!UserService.UserIsAuthenticated(token))
			//{
   //             filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
   //                 {
   //                     action = "Logout",
   //                     controller = "Account"
   //                 }));
			//}
			//else if (!_configProcessor.AccountIsAvailable(token))
			//{
			//	filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
			//	{
			//		action = "Accounts",
			//		controller = "Maintenance"
			//	}));
			//}
			//else if (!_configProcessor.ConfigurationIsValid(token))
			//{
			//	filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
			//		{
			//			action = "Configuration", 
			//			controller = "Maintenance"
			//		}));
			//}
			//base.OnActionExecuting(filterContext);
		}
	}
}