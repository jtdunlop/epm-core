namespace DBSoft.EPM.DAL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Aspects;
    using CodeFirst.Models;
	using Commands;
    using DbSoft.Cache.Aspect.Attributes;
    using Enums;
	using Exceptions;
    using Extensions;
    using Interfaces;
    using NLog;
    using Queries;
	using Requests;
	using System.Linq;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class BlueprintInstanceService : IBlueprintInstanceService
    {
		private readonly IDbContextFactory _factory;
		private readonly IConfigurationService _config;
        private readonly IUserService _users;

        public BlueprintInstanceService(IDbContextFactory factory, IConfigurationService config, IUserService users)
		{
			_factory = factory;
			_config = config;
            _users = users;
		}

		public void SaveBlueprintInstance(SaveBlueprintInstanceRequest request)
		{
            var context = _factory.CreateContext();
			using ( var cmd = new DataCommand(context) )
			{
                var blueprint = cmd.Get<Blueprint>(f => f.ItemID == request.BlueprintItemID);
                if (blueprint.ID == 0)
                {
                    throw new ReferentialIntegrityException(
                        $"Blueprint {request.BlueprintItemID} not found saving instance.");
                }
                if ( !context.Set<Asset>().Any(f => f.ID == request.AssetID))
                {
                    return;
                }
                var instance = cmd.Get<BlueprintInstance>(f => f.AssetID == request.AssetID);

                instance.AssetID = request.AssetID;
                instance.BlueprintID = blueprint.ID;
                instance.DeletedFlag = false;
                instance.IsCopy = request.IsCopy;

                // When ME and PE are passed as null do not overwrite existing instance values as they can only be 
                // populated while the instance is installed in a factory slot, so in this case null means unknown rather than not set.
                if (request.MaterialEfficiency.HasValue)
                {
                    instance.MaterialEfficiency = request.MaterialEfficiency.Value;
                }
                if (request.ProductionEfficiency.HasValue)
                {
                    instance.ProductionEfficiency = request.ProductionEfficiency.Value;
                }
			    try
			    {
                    context.SaveChanges();
                }
			    catch (Exception e)
			    {
                    LogManager.GetCurrentClassLogger().Error(e);
			    }
			}
		}

		public async Task DeleteAll(string token)
		{
			using (var context = _factory.CreateContext())
			{
				var userId = _users.GetUserID(token);
				var instances = context.Set<BlueprintInstance>().Where(f => !f.DeletedFlag && f.Asset.Account.UserID == userId);
				foreach (var instance in instances)
				{
					instance.DeletedFlag = true;
				}
				await context.SaveChangesAsync();
			}
		}

        [Trace,Cache.Cacheable]
        public List<BlueprintInstanceDto> ListBuildable(string token)
        {
            var instances = GetList(token).ToList();
            return instances;
        }

        [Trace,Cache.Cacheable]
        public List<BlueprintInstanceDto> ListDistinctBuildable(string token)
        {
            var instances = GetList(token)
                .DistinctBy(f => f.BuildItemID)
                .ToList();
            return instances;
        }

        private IEnumerable<BlueprintInstanceDto> GetList(string token)
        {
            var user = _users.GetUserID(token);
            var location = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
            var pos = _config.GetSetting<int>(token, ConfigurationType.PosLocation);
            var instances = new DataQuery<BlueprintInstance>(_factory.CreateContext())
                .Specify(f => pos == 0 ? f.Asset.LocationID == location : f.Asset.SolarSystemID == pos)
                .Specify(f => !f.DeletedFlag && f.Asset.UserID == user)
                .GetQuery()
                .Select(f => new BlueprintInstanceDto
                {
                    BuildItemID = f.Blueprint.BuildItemID,
                    BuildItemName = f.Blueprint.BuildItem.Name,
                    ProductionTime = f.Blueprint.ProductionTime
                });
            return instances;
        }

        [Trace, Cache.Cacheable]
        public List<BlueprintInstanceDto> ListDistinctOwned(string token)
        {
            var user = _users.GetUserID(token);
            var context = _factory.CreateContext();

            // Instances I have as assets
            var instances = new DataQuery<BlueprintInstance>(context)
                .Specify(f => !f.DeletedFlag && f.Asset.UserID == user)
                .GetQuery()
                .Select(f => new BlueprintInstanceDto
                {
                    BuildItemID = f.Blueprint.BuildItemID,
                    BuildItemName = f.Blueprint.BuildItem.Name,
                    ProductionTime = f.Blueprint.ProductionTime
                }).ToList();

            // Instances building an item
            var r1 = new DataQuery<ProductionJob>(context)
                .Specify(f => f.Status == ProductionJobStatus.Active)
                .Specify(f => f.UserID == user)
                .Specify(f => f.Item.BuildBlueprints.Any())
                .GetQuery()
                .Select(f => new BlueprintInstanceDto
                {
                    BuildItemID = f.ItemID,
                    BuildItemName = f.Item.Name
                }).ToList();
            instances.AddRange(r1);

            // Instances researching
            var r2 = new DataQuery<ProductionJob>(context)
                .Specify(f => f.Status == ProductionJobStatus.Active)
                .Specify(f => f.UserID == user)
                .Specify(f => f.Item.Blueprints.Any())
                .GetQuery()
                .Select(f => new BlueprintInstanceDto
                {
                    BuildItemID = f.Item.Blueprints.FirstOrDefault().BuildItemID,
                    BuildItemName = f.Item.Blueprints.FirstOrDefault().BuildItem.Name
                }).ToList();
            instances.AddRange(r2);


            return instances.DistinctBy(f => f.BuildItemID).ToList();
        }

    }

}
