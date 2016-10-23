namespace DBSoft.EVEAPI.Crest.SolarSystem
{
    using System.Collections.Generic;
    using Entities.SolarSystem;

    public class SolarSystemResponse
    {
        public string totalCount_str { get; set; }
        public List<Item> items { get; set; }
        public int pageCount { get; set; }
        public string pageCount_str { get; set; }
        public int totalCount { get; set; }
    }
}