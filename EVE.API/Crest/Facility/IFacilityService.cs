namespace DBSoft.EVEAPI.Crest.Facility
{
    using System.Collections.Generic;
    using Entities.Facility;

    public interface IFacilityService
    {
        IEnumerable<FacilityDTO> GetFacilities();
    }
}