namespace DBSoft.EVEAPI.Crest.Inventory
{
    using System.Collections.Generic;

    public class ItemType
    {
        public string id_str { get; set; }
        public string href { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Next
    {
        public string href { get; set; }
    }

    public class ItemTypes
    {
        public string totalCount_str { get; set; }
        public int pageCount { get; set; }
        public List<ItemType> items { get; set; }
        public Next next { get; set; }
        public int totalCount { get; set; }
        public string pageCount_str { get; set; }
    }
}
