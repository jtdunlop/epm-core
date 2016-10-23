using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.ApiVerificationCode)
                .IsRequired()
                .HasMaxLength(65);

            // Table & Column Mappings
            this.ToTable("Account");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ApiKeyID).HasColumnName("ApiKeyID");
            this.Property(t => t.ApiVerificationCode).HasColumnName("ApiVerificationCode");
            this.Property(t => t.ApiKeyType).HasColumnName("ApiKeyType");
            this.Property(t => t.ApiAccessMask).HasColumnName("ApiAccessMask");
            this.Property(t => t.DeletedFlag).HasColumnName("DeletedFlag");
            this.Property(t => t.UserID).HasColumnName("UserID");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Accounts)
                .HasForeignKey(d => d.UserID);

        }
    }
}
