namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    public class TransactionMap : EntityTypeConfiguration<Transaction>
    {
        public TransactionMap()
        {
            HasRequired(t => t.User)
                .WithMany(t => t.Transactions)
                .HasForeignKey(d => d.UserID);

			HasRequired(t => t.Item)
				.WithMany(t => t.Transactions)
				.HasForeignKey(d => d.ItemID);

            Property(p => p.EveTransactionID).HasIndex();

            Property(p => p.TransactionType).HasIndex();

        }
    }
}
