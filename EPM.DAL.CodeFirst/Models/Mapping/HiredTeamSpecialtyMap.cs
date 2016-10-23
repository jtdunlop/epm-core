namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using DBSoft.EPM.DAL.CodeFirst.Models;

    class HiredTeamSpecialtyMap : EntityTypeConfiguration<HiredTeamSpecialty>
    {
        public HiredTeamSpecialtyMap()
        {
            HasKey(k => k.ID);

            HasRequired(r => r.HiredTeam)
                .WithMany(m => m.HiredTeamSpecialties)
                .HasForeignKey(k => k.HiredTeamID);
        }
    }
}
