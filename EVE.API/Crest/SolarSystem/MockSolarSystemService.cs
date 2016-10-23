namespace DBSoft.EVEAPI.Crest.SolarSystem
{
    using System.Collections.Generic;
    using Annotations;
    using Entities.SolarSystem;

    [UsedImplicitly]
    public class MockSolarSystemService : ISolarSystemService
    {
        public IEnumerable<SolarSystemDTO> GetSolarSystems()
        {
            return ResourceLoader.LoadResource<SolarSystemDTO>("DBSoft.EVEAPI.Crest.SolarSystem.SolarSystems.json");
        }
    }
}
