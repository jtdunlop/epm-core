using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class EveApiStatuMap : EntityTypeConfiguration<EveApiStatus>
    {
        public EveApiStatuMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("EveApiStatus");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CacheExpiry).HasColumnName("CacheExpiry");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Result).HasColumnName("Result");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.EveApiStatus)
                .HasForeignKey(d => d.UserID);

        }
    }
}
