namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Login)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.Password)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("User");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Login).HasColumnName("Login");
            Property(t => t.Password).HasColumnName("Password");
            Property(t => t.AuthenticationFailures).HasColumnName("AuthenticationFailures");
            Property(t => t.LockedUntil).HasColumnName("LockedUntil");
        }
    }
}
