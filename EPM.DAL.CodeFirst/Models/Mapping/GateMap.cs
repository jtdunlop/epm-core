namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    public class GateMap : EntityTypeConfiguration<Gate>
    {
        public GateMap()
        {
            // Primary Key
            HasKey(t => t.ID);
        }
    }
}
