using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class CorporationMap : EntityTypeConfiguration<Corporation>
    {
        public CorporationMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Corporation");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.EveApiID).HasColumnName("EveApiID");
            this.Property(t => t.AccountID).HasColumnName("AccountID");

			// Relationships
			this.HasRequired(t => t.Account)
				.WithMany(t => t.Corporations)
				.HasForeignKey(d => d.AccountID);
        }
    }
}
