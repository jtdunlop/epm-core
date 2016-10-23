namespace DBSoft.EPM.DAL.Services.Materials
{
    using System.Collections.Generic;
    using System.Linq;
    using Aspects;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Enums;
    using Extensions;
    using Interfaces;
    using Queries;

    public class MaterialItemService : IMaterialItemService
	{
        private readonly IDbContextFactory _factory;
        private readonly IUserService _users;
        private readonly IConfigurationService _config;

        public MaterialItemService(IDbContextFactory factory, IUserService users, IConfigurationService config)
		{
		    _factory = factory;
		    _users = users;
            _config = config;
		}

        [Trace, Cache.Cacheable]
        public List<MaterialItemDto> ListBuildable(string token)
        {
            using (var context = _factory.CreateContext())
            {
                var userId = _users.GetUserID(token);
                var manifests = new DataQuery<Manifest>(context)
                    .GetQuery()
                    .Select(f => new
                    {
                        ItemId = f.ItemID,
                        ItemName = f.Item.Name,
                        MaterialId = f.MaterialItemID,
                        MaterialName = f.MaterialItem.Name,
                        f.MaterialItem.Volume
                    })
                    .ToList();
                var factory = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
                var pos = _config.GetSetting<int>(token, ConfigurationType.PosLocation);
                var instances = new BlueprintInstanceQuery(context, userId)
                    .SpecifyFactory(pos, factory)
                    .GetQuery()
                    .Select(f => new
                    {
                        f.Blueprint.BuildItemID
                    })
                    .Distinct()
                    .ToList();

                var materials = (from manifest in manifests
                    join instance in instances on manifest.ItemId equals instance.BuildItemID
                    from extension in
                        context.ItemExtensions
                            .Where(f => f.UserID == userId && f.ItemID == instance.BuildItemID)
                            .DefaultIfEmpty()
                    select new MaterialItemDto
                    {
                        ItemId = manifest.MaterialId,
                        ItemName = manifest.MaterialName,
                        BounceFactor = extension == null ? 1 : extension.BounceFactor,
                        LastModified = extension == null ? null : extension.LastModified,
                        Volume = manifest.Volume
                    }).ToList().DistinctBy(f => f.ItemId).ToList();
                return materials.ToList();
            }
        }
	}
}
