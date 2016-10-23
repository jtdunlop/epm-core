namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
	using System.Data.Entity.ModelConfiguration;

	public class BlueprintMap : EntityTypeConfiguration<Blueprint>
    {
        public BlueprintMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("Blueprint");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.ItemID).HasColumnName("ItemID");
            Property(t => t.ProductionTime).HasColumnName("ProductionTime");
            Property(t => t.BuildItemID).HasColumnName("BuildItemID");

            // Relationships
            HasOptional(t => t.BuildItem)
                .WithMany(t => t.BuildBlueprints)
                .HasForeignKey(d => d.BuildItemID);
            HasRequired(t => t.Item)
                .WithMany(t => t.Blueprints)
                .HasForeignKey(d => d.ItemID);

        }
    }
}
