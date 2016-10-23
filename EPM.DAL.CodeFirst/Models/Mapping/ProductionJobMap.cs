using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProductionJobMap : EntityTypeConfiguration<ProductionJob>
    {
        public ProductionJobMap()
        {
            Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Status).HasIndex();

            // Relationships
            HasOptional(t => t.Asset)
                .WithMany(t => t.ProductionJobs)
                .HasForeignKey(d => d.AssetID);
            HasRequired(t => t.Item)
                .WithMany(t => t.ProductionJobs)
                .HasForeignKey(d => d.ItemID);

            HasOptional(t => t.HiredTeam)
                .WithMany(t => t.ProductionJobs)
                .HasForeignKey(d => d.HiredTeamID);

            HasOptional(t => t.User)
                .WithMany(t => t.ProductionJobs)
                .HasForeignKey(d => d.UserID);

        }
    }
}
