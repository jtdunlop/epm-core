using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class SolarSystemMap : EntityTypeConfiguration<SolarSystem>
    {
        public SolarSystemMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            Property(p => p.ManufacturingCost)
                .HasPrecision(16, 4);

            // Relationships
            HasRequired(t => t.Region)
                .WithMany(t => t.SolarSystems)
                .HasForeignKey(d => d.RegionID);

            HasMany(t => t.Gates)
                .WithRequired(r => r.SolarSystem)
                .HasForeignKey(k => k.SolarSystemID);

            HasMany(t => t.Gates)
                .WithRequired(r => r.TargetSolarSystem)
                .HasForeignKey(k => k.SolarSystemID);
        }
    }
}
