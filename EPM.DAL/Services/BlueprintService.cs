namespace DBSoft.EPM.DAL.Services
{
    using System.Collections.Generic;
    using Annotations;
    using Aspects;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using Interfaces;
    using Market;
    using Queries;
    using System.Linq;

    [UsedImplicitly]
    public class BlueprintService : IBlueprintService
    {
        private readonly IDbContextFactory _factory;

        public BlueprintService(IDbContextFactory factory)
        {
            _factory = factory;
        }

        [Trace, Cache.Cacheable]
        public List<BlueprintDTO> List()
        {
            var blueprints = new DataQuery<Blueprint>(_factory.CreateContext())
                .GetQuery()
                .Where(f => f.BuildItemID != null && f.ProductionTime != 0 && f.Item.IsPublished)
                .Select(f => new BlueprintDTO
                {
                    ItemID = f.ItemID,
                    BuildItemID = f.BuildItemID.Value,
                    ProductionTime = f.ProductionTime
                })
                .ToList();
            return blueprints;
        }
    }

}
