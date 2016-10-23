using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class GroupMap : EntityTypeConfiguration<Group>
    {
        public GroupMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Group");
            this.Property(t => t.ID).HasColumnName("ID");
			this.Property(t => t.Name).IsRequired().HasColumnName("Name").HasMaxLength(200);
            this.Property(t => t.CategoryID).HasColumnName("CategoryID");

            // Relationships
            this.HasOptional(t => t.Category)
                .WithMany(t => t.Groups)
                .HasForeignKey(d => d.CategoryID);

        }
    }
}
