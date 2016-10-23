namespace DBSoft.EPM.DAL.Services
{
    using System;
    using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
	using CodeFirst.Models;
	using Commands;
	using DTOs;
	using Interfaces;
	using Queries;
	using Requests;

	public class AccountBalanceService : IAccountBalanceService
	{
	    private readonly IDbContextFactory _factory;
	    private readonly IUserService _users;
	    public const int MasterWallet = 1000;

		private readonly DbContext _context;

		public AccountBalanceService(IDbContextFactory factory, IUserService users)
		{
		    _factory = factory;
		    _users = users;
		    _context = factory.CreateContext();
		}

	    public IEnumerable<AccountBalanceDTO> List(string token)
		{
			var userID = _users.GetUserID(token);

			var result = new DataQuery<AccountBalance>(_context)
				.Specify(f => f.Account.UserID == userID )
				.Specify(f => !f.Account.DeletedFlag)
				.GetQuery()
				.Select(f => new AccountBalanceDTO
				{
					AccountID = f.AccountID,
					AccountKey = f.AccountKey,
					Balance = f.Balance,
				}).ToList();
			return result;
		}

		public void UpdateBalances(AccountBalanceUpdateRequest request)
		{
            using ( var context = _factory.CreateContext() )
            {
                foreach ( var update in request.BalanceUpdates )
                {
                    var accountBalance = context.Get<AccountBalance>(f => f.AccountID == update.AccountID && f.AccountKey == update.AccountKey);
                    accountBalance.AccountID = update.AccountID;
                    accountBalance.AccountKey = update.AccountKey;
                    accountBalance.Balance = update.Balance;
                }
                context.SaveChanges();
            }
		}
	}

    public static class ContextExtensions
    {
        public static T Get<T>(this DbContext context, Expression<Func<T, bool>> expr) where T : class, new()
        {
            var entity = context.Set<T>().SingleOrDefault(expr) ?? context.Set<T>().Add(new T());
            return entity;
        }
    }
}
