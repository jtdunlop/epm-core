namespace DBSoft.EPM.DAL.Factories
{
    using CodeFirst.Models;
    using Interfaces;

    public class EpmEntitiesFactory : IDbContextFactory
	{
		private readonly string _connectionString;

		public EpmEntitiesFactory(IConnectionStringProvider provider)
		{
			_connectionString = provider.ConnectionString;
		}

		public EPMContext CreateContext()
		{
			return new EPMContext(_connectionString);
		}
	}
}
