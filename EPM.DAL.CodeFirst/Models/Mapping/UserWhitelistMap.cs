using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    using System.Data.Entity.Infrastructure.Annotations;

    public class UserWhitelistMap : EntityTypeConfiguration<UserWhitelist>
    {
        public UserWhitelistMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Login)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("UserWhitelist");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Login).HasColumnAnnotation("Index",
                new IndexAnnotation(new[] 
                { 
                    new IndexAttribute("UQ_Login") { IsUnique = true } 
                }));
            Property(t => t.Enabled).HasColumnName("Enabled");
        }
    }
}
