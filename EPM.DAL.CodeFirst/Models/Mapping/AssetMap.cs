using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class AssetMap : EntityTypeConfiguration<Asset>
    {
        public AssetMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("Asset");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.ItemID).HasColumnName("ItemID");
            Property(t => t.Quantity).HasColumnName("Quantity");
            Property(t => t.AccountID).HasColumnName("AccountID");
            Property(t => t.ContainerID).HasColumnName("ContainerID");
            Property(t => t.DeletedFlag).HasColumnName("DeletedFlag");
            Property(t => t.StationID).HasColumnName("StationID");
            Property(t => t.LocationID).HasColumnName("LocationID");
            Property(t => t.UserID).HasColumnName("UserID");

            // Relationships
            HasRequired(t => t.Account)
                .WithMany(t => t.Assets)
                .HasForeignKey(d => d.AccountID);
            HasRequired(t => t.Item)
                .WithMany(t => t.Assets)
                .HasForeignKey(d => d.ItemID);
            HasOptional(t => t.SolarSystem)
                .WithMany(t => t.Assets)
                .HasForeignKey((d => d.SolarSystemID));
            HasRequired(t => t.User)
                .WithMany(t => t.Assets)
                .HasForeignKey(d => d.UserID);

        }
    }
}
