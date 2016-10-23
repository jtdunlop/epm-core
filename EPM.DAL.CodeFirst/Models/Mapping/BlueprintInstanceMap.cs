using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class BlueprintInstanceMap : EntityTypeConfiguration<BlueprintInstance>
    {
        public BlueprintInstanceMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("BlueprintInstance");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AssetID).HasColumnName("AssetID");
            this.Property(t => t.MaterialEfficiency).HasColumnName("MaterialEfficiency");
            this.Property(t => t.ProductionEfficiency).HasColumnName("ProductionEfficiency");
            this.Property(t => t.DeletedFlag).HasColumnName("DeletedFlag");
            this.Property(t => t.BlueprintID).HasColumnName("BlueprintID");
            this.Property(t => t.IsCopy).HasColumnName("IsCopy");

            // Relationships
            this.HasRequired(t => t.Asset)
                .WithMany(t => t.BlueprintInstances)
                .HasForeignKey(d => d.AssetID);
            this.HasRequired(t => t.Blueprint)
                .WithMany(t => t.BlueprintInstances)
                .HasForeignKey(d => d.BlueprintID);

        }
    }
}
