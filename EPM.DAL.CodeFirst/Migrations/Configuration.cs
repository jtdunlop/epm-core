namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
	using Models;
	using System.Data.Entity.Migrations;
	using Operations;

	public class Configuration : DbMigrationsConfiguration<EPMContext>
    {
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
            CommandTimeout = 120;
			SetSqlGenerator("System.Data.SqlClient", new EPMMigrationSqlGenerator());
		}

		protected override void Seed(EPMContext context)
        {
        }
    }
}
