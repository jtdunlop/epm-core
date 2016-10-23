using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class CharacterMap : EntityTypeConfiguration<Character>
    {
        public CharacterMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Character");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.AccountID).HasColumnName("AccountID");
            this.Property(t => t.CorporationID).HasColumnName("CorporationID");
            this.Property(t => t.EveApiID).HasColumnName("EveApiID");

            // Relationships
            this.HasRequired(t => t.Account)
                .WithMany(t => t.Characters)
                .HasForeignKey(d => d.AccountID);

        }
    }
}
