namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    public class HiredTeamMap : EntityTypeConfiguration<HiredTeam>
    {
        public HiredTeamMap()
        {
            HasKey(t => t.ID);

            ToTable("HiredTeam");


        }
    }
}
