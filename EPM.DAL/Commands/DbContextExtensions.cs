namespace DBSoft.EPM.DAL.Commands
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Text;

    public static class DbContextExtensions
    {
        public static void SaveChangesWithErrors(this DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                var sb = new StringBuilder();

                foreach (var failure in e.Entries )
                {
                    sb.AppendFormat("{0} failed update\n", failure.Entity.GetType());
                }

                throw;
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb, ex
                    ); // Add the original exception as the innerException
            }
        }
    }
}