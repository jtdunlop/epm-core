namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class BlueprintInstance
    {
        public long ID { get; set; }
        public long AssetID { get; set; }
        public int MaterialEfficiency { get; set; }
        public int ProductionEfficiency { get; set; }
        public bool DeletedFlag { get; set; }
        public long BlueprintID { get; set; }
        public bool IsCopy { get; set; }
        public virtual Asset Asset { get; set; }
        public virtual Blueprint Blueprint { get; set; }
    }
}
