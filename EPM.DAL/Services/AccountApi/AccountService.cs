namespace DBSoft.EPM.DAL.Services.AccountApi
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Annotations;
    using Aspects;
    using AutoMapper;
    using CodeFirst.Models;
    using Commands;
    using DbSoft.Cache.Aspect.Attributes;
    using DbSoft.Cache.Aspect.Supporting;
    using Interfaces;
    using Queries;
    using Requests;

    [UsedImplicitly]
    public class AccountService : IAccountService
    {
        private readonly IUserService _users;
        private readonly DbContext _context;

	    public AccountService(IDbContextFactory factory, IUserService users)
		{
	        _users = users;
	        _context = factory.CreateContext();
		}

		[Trace,Cache.Cacheable]
		public IEnumerable<AccountDTO> List(AccountRequest request)
		{
			ValidateRequest(request);
			var userID = _users.GetUserID(request.Token);
			var query = new DataQuery<Account>(_context);
			if ( !request.IncludeDeleted )
			{
				query.Specify(f => !f.DeletedFlag);
			}
			if (request.AccountType.HasValue)
			{
				query.Specify(f => f.ApiKeyType == request.AccountType.Value);
			}
				
			return query
				.Specify(f => f.UserID == userID)
				.GetQuery()
				.Select(f => new AccountDTO
			{
				AccountID = f.ID,
				ApiKeyID = f.ApiKeyID,
				ApiVerificationCode = f.ApiVerificationCode,
				ApiAccessMask = f.ApiAccessMask,
				AccountName = f.Name,
				DeletedFlag = f.DeletedFlag,
				ApiKeyType = f.ApiKeyType
			}).ToList();
		}

        private static void ValidateRequest(AccountRequest request)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(request.Token));
        }

        [Trace,Cache.TriggerInvalidation(DeleteSettings.Token, AspectPriority = 2)]
		public void DeleteAccount(DeleteAccountRequest request)
		{
			if (request.AccountID == 0)
			{
				return;
			}

			var userID = _users.GetUserID(request.Token);
			using (var cmd = new DataCommand(_context))
			{
				var account = cmd.Get<Account>(request.AccountID);
				if (account.UserID != userID)
				{
					throw new AccessDeniedException(string.Format("User {0} doesn't have permission to delete accounts for user {1}", userID, account.UserID));
				}
				account.DeletedFlag = true;
				foreach (var character in account.Characters.ToList())
				{
					cmd.Remove<Character>(character.ID);
				}
				foreach (var corporation in account.Corporations.ToList())
				{
					cmd.Remove<Corporation>(corporation.ID);
				}
			}

		}

		[Trace,Cache.TriggerInvalidation(DeleteSettings.Token)]
		public void SaveAccount(SaveAccountRequest request)
		{
			ValidateRequest(request);

			var userID = _users.GetUserID(request.Token);
			using (var cmd = new DataCommand(_context))
			{
				// Since we don't know if this is an add or edit use the lambda version to create a new item if needed
				var account = cmd.Get<Account>(f => f.ID == request.AccountID);
				Mapper.Map(request, account);
				account.UserID = userID;

				if (request.Characters != null)
				{
					var remove = account.Characters.Where(f => request.Characters.All(g => g.EveCharacterID != f.EveApiID)).ToList();
					foreach (var r in remove)
					{
						cmd.Remove<Character>(r.ID);
					}
					var insert = request.Characters.Where(f => account.Characters.All(g => g.EveApiID != f.EveCharacterID)).ToList();
					foreach (var i in insert)
					{
						account.Characters.Add(new Character
							{
								EveApiID = i.EveCharacterID,
								Name = i.CharacterName
							});
					}
				}

				if (request.Corporations != null)
				{
					var remove = account.Corporations.Where(f => request.Corporations.All(g => g.EveCorporationID != f.EveApiID)).ToList();
					foreach ( var r in remove )
					{
						cmd.Remove<Corporation>(r.ID);
					}
					var insert = request.Corporations.Where(f => account.Corporations.All(g => g.EveApiID != f.EveCorporationID)).ToList();
					foreach ( var i in insert )
					{
						account.Corporations.Add(new Corporation
						{
							EveApiID = i.EveCorporationID,
							Name = i.CorporationName
						});
					}
				}
			}
		}

        private static void ValidateRequest(SaveAccountRequest request)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(request.Token));
        }
    }
	public class AccessDeniedException : Exception
	{
		public AccessDeniedException(string message) : base(message)
		{
		}
	}
}
