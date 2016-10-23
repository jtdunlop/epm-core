using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class MarketOrderMap : EntityTypeConfiguration<MarketOrder>
    {
        public MarketOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("MarketOrder");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.EveMarketOrderID).HasColumnName("EveMarketOrderID");
            this.Property(t => t.ItemID).HasColumnName("ItemID");
            this.Property(t => t.EveCharacterID).HasColumnName("EveCharacterID");
            this.Property(t => t.StationID).HasColumnName("StationID");
            this.Property(t => t.OriginalQuantity).HasColumnName("OriginalQuantity");
            this.Property(t => t.RemainingQuantity).HasColumnName("RemainingQuantity");
            this.Property(t => t.MinimumQuantity).HasColumnName("MinimumQuantity");
            this.Property(t => t.OrderStatus).HasColumnName("OrderStatus");
            this.Property(t => t.Range).HasColumnName("Range");
            this.Property(t => t.Duration).HasColumnName("Duration");
            this.Property(t => t.Escrow).HasColumnName("Escrow");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.OrderType).HasColumnName("OrderType");
            this.Property(t => t.WhenIssued).HasColumnName("WhenIssued");
            this.Property(t => t.CharacterID).HasColumnName("CharacterID");
            this.Property(t => t.UserID).HasColumnName("UserID");

            // Relationships
            this.HasRequired(t => t.Item)
                .WithMany(t => t.MarketOrders)
                .HasForeignKey(d => d.ItemID);
            this.HasOptional(t => t.User)
                .WithMany(t => t.MarketOrders)
                .HasForeignKey(d => d.UserID);

        }
    }
}
