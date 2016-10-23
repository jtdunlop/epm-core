namespace DBSoft.EVEAPI.Crest.Facility
{
    using System.Collections.Generic;
    using System.Linq;
    using Annotations;
    using DbSoft.Cache.Aspect.Attributes;
    using Entities;
    using Entities.Facility;
    using EveAPI.Crest.Root;
    using Newtonsoft.Json;

    [UsedImplicitly]
    public class FacilityService : IFacilityService
    {
        [Cache.Cacheable]
        public IEnumerable<FacilityDTO> GetFacilities()
        {
            var url = CrestService.GetAuthEndpoints().Industry.Facilities.Href;

            var readToEnd = JsonLoader.Load(url);
            var result = JsonConvert.DeserializeObject<Facility>(readToEnd);
            return result.items.Select(f => new FacilityDTO
            {
                FacilityId = f.facilityID,
                FacilityName = f.name,
                SolarSystemId = f.solarSystem.id,
                Tax = (decimal)f.tax
            }).ToList();
        }
    }

}

