namespace DBSoft.EPM.DAL
{
	using System.Security.Cryptography.X509Certificates;

	public interface IConnectionStringProvider
	{
		string ConnectionString { get; }
	}
}