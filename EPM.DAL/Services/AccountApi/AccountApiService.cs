using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DBSoft.EPM.DAL.CodeFirst.Models;
using DBSoft.EPM.DAL.Interfaces;
using DBSoft.EPM.DAL.Queries;
using DBSoft.EVEAPI.Entities.Account;

namespace DBSoft.EPM.DAL.Services.AccountApi
{
    using Aspects;
    using DbSoft.Cache.Aspect.Attributes;

    public class AccountApiService : IAccountApiService
    {
        private readonly IUserService _users;
        private readonly DbContext _context;

        public AccountApiService(IDbContextFactory factory, IUserService users)
        {
            _users = users;
            _context = factory.CreateContext();
        }

        [Trace,Cache.Cacheable]
        public IEnumerable<AccountApiDTO> List(string token)
        {
            var userID = _users.GetUserID(token);

            var query = new DataQuery<Character>(_context)
                .Specify(f => !f.Account.DeletedFlag && f.Account.UserID == userID);

            var result = query.GetQuery().Select(f => new AccountApiDTO
            {
                ApiID = f.ID,
                ApiKeyType = ApiKeyType.Character,
                AccountID = f.AccountID,
                EveApiID = f.EveApiID,
                ApiKeyID = f.Account.ApiKeyID,
                ApiVerificationCode = f.Account.ApiVerificationCode,
                AccountName = f.Account.Name
            }).ToList();
            
            result.AddRange(new DataQuery<Corporation>(_context).GetQuery()
                .Where(f => f.Account.UserID == userID).Select(f => new AccountApiDTO
                {
                    ApiID = f.ID,
                    ApiKeyType = ApiKeyType.Corporation,
                    AccountID = f.AccountID.Value,
                    ApiKeyID = f.Account.ApiKeyID,
                    ApiVerificationCode = f.Account.ApiVerificationCode,
                    EveApiID = f.EveApiID,
                    AccountName = f.Account.Name
                }).ToList());
            return result;
        }
    }
}