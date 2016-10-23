using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class EveAccountMap : EntityTypeConfiguration<EveAccount>
    {
        public EveAccountMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.EveCharacterName)
                .HasMaxLength(50);

            // Relationships
            HasRequired(t => t.User)
                .WithMany(t => t.EveAccounts)
                .HasForeignKey(d => d.UserID);

        }
    }
}
