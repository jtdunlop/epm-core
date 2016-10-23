namespace DBSoft.EVEAPI.Crest.Facility
{
    using System.Collections.Generic;
    using Annotations;
    using Entities.Facility;

    [UsedImplicitly]
    public class MockFacilityService : IFacilityService
    {
        public IEnumerable<FacilityDTO> GetFacilities()
        {
            var foo =  ResourceLoader.LoadResource<FacilityDTO>("DBSoft.EVEAPI.Crest.Facility.Facilities.json");
            return foo;
        }
    }
}
