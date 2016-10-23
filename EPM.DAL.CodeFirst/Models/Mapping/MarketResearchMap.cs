using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class MarketResearchMap : EntityTypeConfiguration<MarketResearch>
    {
        public MarketResearchMap()
        {
            // Primary Key
            HasKey(t => t.ID);
        }
    }
}
