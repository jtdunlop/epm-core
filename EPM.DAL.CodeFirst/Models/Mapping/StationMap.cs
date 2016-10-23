using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class StationMap : EntityTypeConfiguration<Station>
    {
        public StationMap()
        {
            Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Relationships
            HasRequired(t => t.SolarSystem)
                .WithMany(t => t.Stations)
                .HasForeignKey(d => d.SolarSystemID);

        }
    }
}
