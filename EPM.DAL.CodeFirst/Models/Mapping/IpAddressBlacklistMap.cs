using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class IpAddressBlacklistMap : EntityTypeConfiguration<IpAddressBlacklist>
    {
        public IpAddressBlacklistMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.IpAddress)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("IpAddressBlacklist");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IpAddress).HasColumnName("IpAddress");
            this.Property(t => t.Count).HasColumnName("Count");
        }
    }
}
