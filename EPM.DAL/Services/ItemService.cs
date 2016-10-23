namespace DBSoft.EPM.DAL.Services
{
    using System;
    using Aspects;
    using AutoMapper;
    using CodeFirst.Models;
    using Commands;
    using DbSoft.Cache.Aspect.Attributes;
    using DbSoft.Cache.Aspect.Supporting;
    using DTOs;
    using Enums;
    using Extensions;
    using Interfaces;
    using Market;
    using Queries;
    using Requests;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using DbSoft.Cache.Aspect;

    public class ItemService : IItemService
    {
        private readonly IDbContextFactory _factory;
        private readonly IConfigurationService _config;
        private readonly IBlueprintService _blueprints;
        private readonly IUserService _users;
        private readonly IItemExtensionQueryFactory _extensions;
        private readonly IBlueprintInstanceQueryFactory _instances;

        public ItemService(IDbContextFactory factory, IConfigurationService config, IBlueprintService blueprints,
            IUserService users, IItemExtensionQueryFactory extensions, IBlueprintInstanceQueryFactory instances)
        {
            _factory = factory;
            _config = config;
            _blueprints = blueprints;
            _users = users;
            _extensions = extensions;
            _instances = instances;
        }

        [Trace,Cache.Cacheable]
        public List<ItemDTO> List(ItemRequest request)
        {
            using (var context = _factory.CreateContext())
            {
                var query = new DataQuery<Item>(context);

                if (request.CategoryId.HasValue)
                {
                    query.Specify(f => f.Group.CategoryID == request.CategoryId.Value);
                }
                var result = query.GetQuery().Select(f => new ItemDTO
                {
                    ItemID = f.ID,
                    ItemName = f.Name,
                    QuantityMultiplier = f.QuantityMultiplier,
                    Volume = f.Volume
                });
                return result.ToList();
            }
        }

        [Trace,Cache.Cacheable]
        public List<BuildableItemDTO> ListMaintainable(string token)
        {
            using (var context = _factory.CreateContext())
            {
                var userId = _users.GetUserID(token);
                var instances = new BlueprintInstanceQuery(context, userId);
                var stationId = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
                var pos = _config.GetSetting<int>(token, ConfigurationType.PosLocation);
                var items = pos == 0 ?
                    new ListableItemQuery(context, userId).SpecifyStation(stationId).GetQuery() :
                    new ListableItemQuery(context, userId).SpecifySolarSystem(pos, stationId).GetQuery();
                var blueprints = pos == 0 ?
                    instances
                    .SpecifyStation(stationId)
                    .GetQuery() :
                    instances
                    .SpecifySolarSystem(pos, stationId)
                    .GetQuery();

                var result = from item in items
                             from blueprint in blueprints.Where(f => f.Blueprint.BuildItemID == item.ID).DefaultIfEmpty()
                             select new BuildableItemDTO
                             {
                                 ItemID = item.ID,
                                 ItemName = item.Name,
                                 // ReSharper disable MergeConditionalExpression
                                 MaterialEfficiency = blueprint == null ? 10 : blueprint.MaterialEfficiency,
                                 ProductionEfficiency = blueprint == null ? 20 : blueprint.ProductionEfficiency,
                                 // ReSharper restore MergeConditionalExpression
                                 MinimumStock = item.ItemExtensions.FirstOrDefault(f => f.UserID == userId).MinimumStock,
                                 PerJobAdditionalCost = item.ItemExtensions.FirstOrDefault(f => f.UserID == userId).PerJobAdditionalCost,
                                 QuantityMultiplier = item.QuantityMultiplier,
                                 Volume = item.Volume
                             };

                return result.ToList().DistinctBy(f => f.ItemID).ToList();
            }
        }


        [Trace,Cache.Cacheable]
        public List<BuildableItemDTO> ListBuildable(string token)
        {
            using (var context = _factory.CreateContext())
            {
                var stationId = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
                var pos = _config.GetSetting<int>(token, ConfigurationType.PosLocation);
                var userId = _users.GetUserID(token);

                var instances = GetInstances(context, userId, pos, stationId);
                var extensions = _extensions.Create(context, userId).ToList();

                // Orphaned extensions (BPC or removed instance)
                var r1 = from extension in extensions.Where(f => instances.All(a => a.Blueprint.BuildItemID != f.ItemID && f.MinimumStock > 0))
                         select new BuildableItemDTO
                         {
                             ItemID = extension.ItemID,
                             ItemName = extension.Item.Name,
                             MaterialEfficiency = 20,
                             ProductionEfficiency = 10,
                             MinimumStock = extension.MinimumStock,
                             PerJobAdditionalCost = extension.PerJobAdditionalCost,
                             QuantityMultiplier = extension.Item.QuantityMultiplier,
                             Volume = GetPackagedVolume(extension.Item.Volume, extension.Item.GroupID)
                         };
                var r2 = from instance in instances
                         from extension in extensions.Where(f => f.ItemID == instance.Blueprint.BuildItemID).DefaultIfEmpty()
                         select new BuildableItemDTO
                         {
                             ItemID = instance.Blueprint.BuildItem.ID,
                             ItemName = instance.Blueprint.BuildItem.Name,
                             MaterialEfficiency = instance.MaterialEfficiency,
                             ProductionEfficiency = instance.ProductionEfficiency,
                             MinimumStock = extension == null ? 0 : extension.MinimumStock,
                             QuantityMultiplier = instance.Blueprint.BuildItem.QuantityMultiplier,
                             PerJobAdditionalCost = extension == null ? 0 : extension.PerJobAdditionalCost,
                             Volume = GetPackagedVolume(instance.Blueprint.BuildItem.Volume, instance.Blueprint.BuildItem.GroupID)
                         };
                var result = r1.Concat(r2).ToList();
                return result;
            }
        }

        private IEnumerable<BlueprintInstance> GetInstances(EPMContext context, int userId, int pos, int stationId)
        {
            var instances = _instances.Create(context, userId);
            return (pos == 0 ? instances.SpecifyStation(stationId) : instances.SpecifySolarSystem(pos, stationId))
                .GetQuery()
                .ToList()
                .DistinctBy(f => f.Blueprint.BuildItemID).ToList();
        }

        private static decimal GetPackagedVolume(decimal volume, int groupId)
        {
            switch ( groupId )
            {
                case 31:
                    return 500;
                case 25:
                case 324:
                case 830:
                case 893:
                case 831:
                case 834:
                    return 2500;
                case 419:
                case 543:
                case 463:
                    return 3750;
                case 420:
                case 541:
                case 963:
                case 1305:
                    return 5000;
                case 26:
                case 906:
                case 833:
                case 358:
                case 894:
                case 832:
                    return 10000;
                case 1201:
                case 540:
                    return 15000;
                case 28:
                case 1202:
                case 380:
                    return 20000;
                case 27:
                case 898:
                case 900:
                    return 50000;
                case 941:
                    return 500000;
                default:
                    return volume;
            }
        }

        [Trace,Cache.Cacheable]
        public List<ItemDTO> ListProducibleMaterials()
        {
            var items = ListProducible();
            using (var context = _factory.CreateContext())
            {
                var manifests = context.Manifests.Include(f => f.MaterialItem).ToList();
                var result = from item in items
                             join manifest in manifests on item.ItemID equals manifest.ItemID
                             group manifest by new { manifest.MaterialItemID, manifest.MaterialItem.Name, manifest.MaterialItem.Volume } into grp
                             select new ItemDTO
                             {
                                 ItemID = grp.Key.MaterialItemID,
                                 ItemName = grp.Key.Name,
                                 Volume = grp.Key.Volume
                             };
                return result.ToList();
            }
        }

        public void UpdateMaterial(UpdateMaterialRequest request)
        {
            var userId = _users.GetUserID(request.Token);
            using (var context = _factory.CreateContext())
            {
                using (var cmd = new DataCommand(context))
                {
                    var extension = cmd.Get<ItemExtension>(f => f.ItemID == request.ItemID && f.UserID == userId);
                    extension.BounceFactor = request.BounceFactor;
                    extension.ItemID = request.ItemID;
                    extension.UserID = userId;
                    extension.LastModified = DateTime.UtcNow;
                }
                CacheService.ClearCache("BuildMaterials");
            }
        }

        [Trace,Cache.TriggerInvalidation(DeleteSettings.Token)]
        public void UpdateItem(UpdateItemRequest request)
        {
            var userId = _users.GetUserID(request.Token);
            using (var context = _factory.CreateContext())
            using (var cmd = new DataCommand(context))
            {
                var extension = cmd.Get<ItemExtension>(f => f.ItemID == request.ItemID && f.UserID == userId);
                extension.MinimumStock = request.MinimumStock;
                extension.PerJobAdditionalCost = request.PerJobAdditionalCost;
                extension.ItemID = request.ItemID;
                extension.UserID = userId;
            }
        }

        [Trace,Cache.Cacheable]
        public List<ItemDTO> ListProducible()
        {
            var blueprints = _blueprints.List();
            using (var context = _factory.CreateContext())
            {
                var query = new DataQuery<Item>(context);
                return query.GetQuery().ToList()
                    .Where(g => blueprints.Any(f => f.BuildItemID == g.ID))
                    .Select(Mapper.Map<ItemDTO>).ToList();
            }
        }
    }
}
