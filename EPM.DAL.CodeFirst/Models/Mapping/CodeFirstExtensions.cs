namespace DBSoft.EPM.DAL.CodeFirst.Models.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration.Configuration;

    public static class CodeFirstExtensions
    {
        public static PrimitivePropertyConfiguration HasIndex(this PrimitivePropertyConfiguration config)
        {
            return config.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
        }
    }
}