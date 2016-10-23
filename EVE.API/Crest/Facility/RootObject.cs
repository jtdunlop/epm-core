namespace DBSoft.EVEAPI.Crest.Facility
{
    using System.Collections.Generic;
    using Entities.Facility;

    public class Facility
    {
        public string totalCount_str { get; set; }
        public List<Item> items { get; set; }
        public int pageCount { get; set; }
        public string pageCount_str { get; set; }
        public int totalCount { get; set; }
    }
}