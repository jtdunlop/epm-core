namespace DBSoft.EVEAPI.Crest.SolarSystem
{
    using System.Collections.Generic;
    using Entities.SolarSystem;

    public interface ISolarSystemService
    {
        IEnumerable<SolarSystemDTO> GetSolarSystems();
    }
}