namespace DBSoft.EVEAPI.Crest.SolarSystem
{
    using System.Collections.Generic;
    using System.Linq;
    using Annotations;
    using DbSoft.Cache.Aspect.Attributes;
    using Entities;
    using Entities.SolarSystem;
    using EveAPI.Crest.Root;
    using Newtonsoft.Json;

    [UsedImplicitly]
    public class SolarSystemService : ISolarSystemService
    {
        [Cache.Cacheable(ttl: 3600 * 23), UsedImplicitly]
        public IEnumerable<SolarSystemDTO> GetSolarSystems()
        {
            var v1 = CrestService.GetAuthEndpoints();
            var url = v1.Industry.Systems.Href;
            var json = JsonLoader.Load(url);
            var result = JsonConvert.DeserializeObject<SolarSystemResponse>(json);
            var x = result.items.Select(f =>
            {
                var systemCostIndice = f.systemCostIndices.SingleOrDefault(g => g.activityID == 1);
                return new SolarSystemDTO
                {
                    Name = f.solarSystem.name,
                    Id = f.solarSystem.id,
                    ManufacturingCost = systemCostIndice == null ? (decimal?)null : (decimal)systemCostIndice.costIndex
                };
            }).ToList();
            return x;
        }
    }
}
