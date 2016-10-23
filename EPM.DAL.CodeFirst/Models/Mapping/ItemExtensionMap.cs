using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class ItemExtensionMap : EntityTypeConfiguration<ItemExtension>
    {
        public ItemExtensionMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("ItemExtension");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.MinimumStock).HasColumnName("MinimumStock");
            this.Property(t => t.BounceFactor).HasColumnName("BounceFactor");
            this.Property(t => t.ItemID).HasColumnName("ItemID");
            this.Property(t => t.UserID).HasColumnName("UserID");

            // Relationships
            this.HasRequired(t => t.Item)
                .WithMany(t => t.ItemExtensions)
                .HasForeignKey(d => d.ItemID);
            this.HasOptional(t => t.User)
                .WithMany(t => t.ItemExtensions)
                .HasForeignKey(d => d.UserID);

        }
    }
}
