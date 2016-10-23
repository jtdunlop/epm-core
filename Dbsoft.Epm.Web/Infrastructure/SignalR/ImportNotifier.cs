
namespace Dbsoft.Epm.Web.Infrastructure.SignalR
{
    using System.Collections.Generic;
    using DBSoft.EPM.DAL.Services;
    using DBSoft.EPM.Logic;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Infrastructure;

    public class ImportNotifier : IImportNotifier
    {
        private readonly IUserService _users;
        private readonly List<string> _running;
        private readonly IHubContext _hub;

        public ImportNotifier([FromServices] IConnectionManager connectionManager, IUserService users)
        {
            _users = users;
            _running = new List<string>();
            _hub = connectionManager.GetHubContext<RefreshHub>();
        }

        public void Start(string token, string name)
        {
            _running.Add(name);
            Notify(token);
        }

        public void Stop(string token, string name)
        {
            _running.RemoveAll(f => f == name);
            Notify(token);
        }

        private void Notify(string token)
        {
            var user = _users.GetAuthenticatedUser(token);
            _hub.Clients.User(user.UserName).RefreshUpdated(_running);
        }
    }
}
