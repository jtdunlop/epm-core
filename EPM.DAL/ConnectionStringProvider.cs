using DBSoft.EPM.DAL.Interfaces;

namespace DBSoft.EPM.DAL
{
	using Commands;

	public class ConnectionStringProvider : IConnectionStringProvider
	{
	    private readonly IEpmConfig _epmConfig;

	    public ConnectionStringProvider(IEpmConfig epmConfig)
        {
            _epmConfig = epmConfig;
        }

	    public string ConnectionString => new ConnectionStringBuilder(_epmConfig).Build();
	}
}
