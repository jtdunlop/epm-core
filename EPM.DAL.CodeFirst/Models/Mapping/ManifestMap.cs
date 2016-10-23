using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class ManifestMap : EntityTypeConfiguration<Manifest>
    {
        public ManifestMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Manifest");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ItemID).HasColumnName("ItemID");
            this.Property(t => t.MaterialItemID).HasColumnName("MaterialItemID");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.AdditionalQuantity).HasColumnName("AdditionalQuantity");

            // Relationships
            this.HasRequired(t => t.Item)
                .WithMany(t => t.Manifests)
                .HasForeignKey(d => d.ItemID);
            this.HasRequired(t => t.MaterialItem)
                .WithMany(t => t.Manifests1)
                .HasForeignKey(d => d.MaterialItemID);

        }
    }
}
