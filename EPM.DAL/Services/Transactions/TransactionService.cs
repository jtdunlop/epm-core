namespace DBSoft.EPM.DAL.Services.Transactions
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using CodeFirst.Models;
    using Interfaces;
    using JetBrains.Annotations;
    using NLog;
    using Queries;

    [UsedImplicitly]
    public class TransactionService : ITransactionService
    {
        private readonly IDbContextFactory _factory;
        private readonly IUserService _users;
        private readonly DbContext _context;
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public TransactionService(IDbContextFactory factory, IUserService users)
		{
		    _factory = factory;
		    _users = users;
		    _context = factory.CreateContext();
		}

		public long GetLastTransactionID(string token)
		{
			var userID = _users.GetUserID(token);
			var query = new DataQuery<Transaction>(_context)
						.Specify(f => f.UserID == userID)
						.GetQuery();
			var result = query.Max(f => (long?)f.EveTransactionID);
			return result ?? 0;
		}

        public async Task SaveTransactions(string token, IEnumerable<SaveTransactionRequest> requests)
		{
			using ( var context = _factory.CreateContext() )
			{
				context.Configuration.AutoDetectChangesEnabled = false;
				var saveTransactionRequests = requests as SaveTransactionRequest[] ?? requests.ToArray();
				if (saveTransactionRequests.Any())
				{
					var insertions = 0;
					var rq = saveTransactionRequests.Length;
					foreach (var request in saveTransactionRequests)
					{
						var userId = _users.GetUserID(token);
						var transactionId = request.EveTransactionID;
						var transaction = context.Get<Transaction>(f => f.EveTransactionID == transactionId && f.UserID == userId);
						// Transactions are immutable so no need to update existing
                        // A change in DateTime comes from a mock service, roll with it
						if (transaction.ID != 0 && transaction.DateTime == request.DateTime) continue;
						insertions++;
						Mapper.Map(request, transaction);
						transaction.UserID = userId;
						if (transaction.TransactionType == TransactionType.Sell)
						{
							transaction.Cost = request.Cost;
						}
					}
					Logger.Trace("User {0} {1} requests {2} insertions", _users.GetUserID(token), rq, insertions);
				}
                context.ChangeTracker.DetectChanges();
                await context.SaveChangesAsync();
			}
		}
	}
}
