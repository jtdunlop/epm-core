namespace DBSoft.EPM.DAL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using AutoMapper;
    using CodeFirst.Models;
    using Commands;
    using DbSoft.Cache.Aspect.Attributes;
    using DbSoft.Cache.Aspect.Supporting;
    using DTOs;
    using Interfaces;
    using Queries;
    using Requests;

    public class UserService : IUserService
    {
		private static readonly Dictionary<string, UserDTO> Sessions = new Dictionary<string, UserDTO>();

		private readonly DataQuery<User> _users;
		private readonly DbContext _context;
        private readonly DataQuery<EveAccount> _accounts;

        public UserService(IDbContextFactory factory)
		{
			_context = factory.CreateContext();
			_users = new DataQuery<User>(_context);
            _accounts = new DataQuery<EveAccount>(_context);
		}

		public static bool UserIsAuthenticated(string token)
		{
			return Sessions.ContainsKey(token);
		}

        private void UpdateLastLogin(int userId, string refresh)
        {
            using ( var cmd = new DataCommand(_context))
            {
                var user = cmd.Get<User>(f => f.ID == userId);
                user.LastLogin = DateTime.UtcNow;
                if (refresh != null)
                {
                    user.SsoRefreshToken = refresh;
                }
            }
        }

        public AuthenticateResponse Authenticate(SsoAuthenticateRequest request)
        {
            if (Sessions.ContainsKey(request.SessionToken)) return new AuthenticateResponse();

            var account = _accounts.Specify(f => f.EveCharacterName == request.EveOnlineCharacter).GetQuery().SingleOrDefault() ??
                          RegisterUser(new RegisterUserRequest { Login = request.EveOnlineCharacter });

            if (!UserIsWhitelisted(account))
            {
                return new AuthenticateResponse
                {
                    ErrorMessage = "User account not pre-authorised. Please contact SJ Astralana."
                };
            }
            UpdateLastLogin(account.UserID, request.RefreshToken);
            Sessions.Add(request.SessionToken, new UserDTO
            {
                UserID = account.UserID,
                UserName = account.EveCharacterName,
                SsoRefreshToken = request.RefreshToken ?? account.User.SsoRefreshToken
            });
            return new AuthenticateResponse();
        }

        [Cache.TriggerInvalidation(DeleteSettings.Token)]
        public void Impersonate(string impersonate, string token)
        {
            var account = _accounts.Specify(f => f.EveCharacterName == impersonate).GetQuery().SingleOrDefault();
            if ( account == null )
            {
                throw new Exception($"Account {impersonate} not found");
            }

            var user = account.User;
            Sessions[token].UserID = user.ID;
            Sessions[token].UserName = user.Login;
        }

        private EveAccount RegisterUser(RegisterUserRequest request)
        {
            using (var cmd = new DataCommand(_context))
            {
                var account = cmd.Get<EveAccount>(f => f.EveCharacterName == request.Login);
                if (account.ID != 0)
                    throw new Exception("Duplicate Login");
                account.EveCharacterName = request.Login;
                account.User = new User
                {
                    Login = request.Login
                };

                var whitelist = cmd.Get<UserWhitelist>(f => f.Login == request.Login);
                if (whitelist.ID != 0) return account;
                whitelist.Login = request.Login;
                whitelist.Enabled = true;

                return account;
            }
        }

        public List<UserDTO> List()
        {
            return _users.GetQuery().ToList().Select(Mapper.Map<UserDTO>).ToList();
        }

        public async Task<UserDTO> GetUser(string name)
        {
            return await GetUser(f => f.EveCharacterName == name);
        }

        public async Task<UserDTO> GetUser(int id)
        {
            return await GetUser(f => f.UserID == id);
        }

        private async Task<UserDTO> GetUser(Expression<Func<EveAccount, bool>> func)
        {
            var user = await _accounts.Specify(func).GetQuery().FirstOrDefaultAsync();
            return user == null ? null : new UserDTO
            {
                UserID = user.UserID,
                UserName = user.EveCharacterName,
                SsoRefreshToken = user.User.SsoRefreshToken
            };
        }

        public int GetUserID(string token)
        {
            return GetAuthenticatedUser(token).UserID;
        }

        public void Logout(string token)
        {
            Sessions.Remove(token);
        }
		
		public UserDTO GetAuthenticatedUser(string token)
		{
			if (token == null || !Sessions.ContainsKey(token))
			{
			    throw new AuthenticationException("Session not found");
			}
			return Sessions[token];
		}

		private bool UserIsWhitelisted(EveAccount account)
		{
			var whitelist = _context.Set<UserWhitelist>().SingleOrDefault(f => f.Login == account.User.Login && f.Enabled);
			return whitelist != null;
		}

        public bool IsAdmin(string token)
        {
            return GetAuthenticatedUser(token).UserID == 1;
        }
    }
}
