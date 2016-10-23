namespace DBSoft.EPM.DAL.Interfaces
{
	using CodeFirst.Models;

    public interface IDbContextFactory
	{
		EPMContext CreateContext();
	}
}
