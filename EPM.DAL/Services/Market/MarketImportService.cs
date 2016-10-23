namespace DBSoft.EPM.DAL.Services.Market
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Annotations;
    using AutoMapper;
    using CodeFirst.Models;
    using EntityFramework.Extensions;
    using Interfaces;
    using NLog;
    using Requests;

    [JetBrains.Annotations.UsedImplicitly]
    public class MarketImportService : IMarketImportService
	{
        private readonly IDbContextFactory _factory;
        private readonly IUserService _users;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); 

		public MarketImportService(IDbContextFactory factory, IUserService users)
		{
		    _factory = factory;
		    _users = users;
		}

		public async Task SaveMarketImports([NotNull] string token, DateTime utcNow, List<SaveMarketImportRequest> requests)
		{
		    await ClearMarketImports(token);

            if ( !requests.Any() )
            {
                return;
            }

            var userId = _users.GetUserID(token);
            using ( var context = _factory.CreateContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                foreach (var request in requests)
                {
                    try
                    {
                        var entity = Mapper.Map<MarketImport2>(request);
                        entity.UserID = userId;
                        entity.TimeStamp = utcNow;
                        context.Set<MarketImport2>().Add(entity);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, "Market Import Error");
                    }
                }
                await context.SaveChangesAsync();
            }
		}

        private async Task ClearMarketImports(string token)
        {
            using ( var context = _factory.CreateContext() )
            {
                context.Database.CommandTimeout = 60;
                var userId = _users.GetUserID(token);
                await context.Set<MarketImport2>().Where(f => f.UserID == userId).DeleteAsync();
            }
        }
	}
}
