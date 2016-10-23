namespace DBSoft.EVEAPI.Entities.Blueprint
{
    public class Blueprint
    {
        public long AssetID { get; set; }
        public long BlueprintID { get; set; }
        public bool IsCopy { get; set; }
        public int TimeEfficiency { get; set; }
        public int MaterialEfficiency { get; set; }
    }
}