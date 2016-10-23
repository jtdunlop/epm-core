namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
	using System.Data.Entity.ModelConfiguration;

	public class MarketPriceMap : EntityTypeConfiguration<MarketPrice>
    {
        public MarketPriceMap()
        {
            HasRequired(t => t.Item)
                .WithMany(t => t.MarketPrices)
                .HasForeignKey(d => d.ItemID);
        }
    }
}
