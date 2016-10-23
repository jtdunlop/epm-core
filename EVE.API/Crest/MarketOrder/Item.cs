namespace DBSoft.EVEAPI.Crest.MarketOrder
{
    public class Item
    {
        public string volume_str { get; set; }
        public bool buy { get; set; }
        public string issued { get; set; }
        public double price { get; set; }
        public int volumeEntered { get; set; }
        public int minVolume { get; set; }
        public long volume { get; set; }
        public string range { get; set; }
        public string href { get; set; }
        public string duration_str { get; set; }
        public Location location { get; set; }
        public int duration { get; set; }
        public string minVolume_str { get; set; }
        public string volumeEntered_str { get; set; }
        public Type type { get; set; }
        public long id { get; set; }
        public string id_str { get; set; }
    }

}