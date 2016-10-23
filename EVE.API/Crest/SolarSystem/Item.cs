namespace DBSoft.EVEAPI.Crest.SolarSystem
{
    using System.Collections.Generic;
    using Entities.SolarSystem;

    public class Item
    {
        public List<SystemCostIndice> systemCostIndices { get; set; }
        public SolarSystem solarSystem { get; set; }
    }
}