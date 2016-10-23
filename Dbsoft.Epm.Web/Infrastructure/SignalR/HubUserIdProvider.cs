using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Http;

namespace Dbsoft.Epm.Web.Infrastructure.SignalR
{
    using System;
    using DBSoft.EPM.DAL.Services;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class HubUserIdProvider : IUserIdProvider
    {
        private readonly IUserService _users;

        public HubUserIdProvider(IUserService users)
        {
            _users = users;
        }

        public string GetUserId(IRequest request)
        {
            var session = request.User.ToString();
            if (session == null) return null;
            var token = new Guid(session).ToString();
            var user = _users.GetAuthenticatedUser(token);
            return user.UserName;
        }
    }
}