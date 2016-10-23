namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Infrastructure.Annotations;
	using System.Data.Entity.ModelConfiguration;

	public class MarketImport2Map : EntityTypeConfiguration<MarketImport2>
    {
        public MarketImport2Map()
        {
            // Table & Column Mappings
            ToTable("MarketImport2");

            // Relationships
            HasRequired(t => t.Item)
                .WithMany(t => t.MarketImports)
                .HasForeignKey(d => d.ItemID);
            HasRequired(t => t.User)
                .WithMany(t => t.MarketImports)
                .HasForeignKey(d => d.UserID);

            Property(p => p.TimeStamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_MarketImport2_Composite", 1)));
            Property(p => p.UserID)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_MarketImport2_Composite", 2)));
            Property(p => p.OrderType)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_MarketImport2_Composite", 3)));
        }
    }

	public class MarketImportMap : EntityTypeConfiguration<MarketImport>
	{
		public MarketImportMap()
		{
			// Primary Key
			HasKey(t => t.ID);

			// Properties
			Property(t => t.ID)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			// Table & Column Mappings
			ToTable("MarketImport");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.TimeStamp).HasColumnName("TimeStamp");
			Property(t => t.StationID).HasColumnName("StationID");
			Property(t => t.SolarSystemID).HasColumnName("SolarSystemID");
			Property(t => t.RegionID).HasColumnName("RegionID");
			Property(t => t.Price).HasColumnName("Price");
			Property(t => t.ItemID).HasColumnName("ItemID");
			Property(t => t.OrderType).HasColumnName("OrderType");
			Property(t => t.Jumps).HasColumnName("Jumps");
			Property(t => t.Range).HasColumnName("Range");
			Property(t => t.UserID).HasColumnName("UserID");
		}
	}

}
