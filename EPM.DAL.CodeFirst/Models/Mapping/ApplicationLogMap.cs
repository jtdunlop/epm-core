using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    public class ApplicationLogMap : EntityTypeConfiguration<ApplicationLog>
    {
        public ApplicationLogMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Origin)
                .IsRequired();

            this.Property(t => t.LogLevel)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Message)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ApplicationLog");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Origin).HasColumnName("Origin");
            this.Property(t => t.LogLevel).HasColumnName("LogLevel");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Exception).HasColumnName("Exception");
            this.Property(t => t.StackTrace).HasColumnName("StackTrace");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
        }
    }
}
