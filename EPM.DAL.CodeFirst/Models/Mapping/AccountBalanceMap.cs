using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class AccountBalanceMap : EntityTypeConfiguration<AccountBalance>
    {
        public AccountBalanceMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("AccountBalance");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AccountID).HasColumnName("AccountID");
            this.Property(t => t.AccountKey).HasColumnName("AccountKey");
            this.Property(t => t.Balance).HasColumnName("Balance");

            // Relationships
            this.HasRequired(t => t.Account)
                .WithMany(t => t.AccountBalances)
                .HasForeignKey(d => d.AccountID);

        }
    }
}
