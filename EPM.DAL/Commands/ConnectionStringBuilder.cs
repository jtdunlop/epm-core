namespace DBSoft.EPM.DAL.Commands
{
    using Interfaces;

	public class ConnectionStringBuilder
	{
	    private readonly IEpmConfig _epmConfig;

	    public ConnectionStringBuilder(IEpmConfig epmConfig)
		{
		    _epmConfig = epmConfig;
		}

	    public string Build()
		{
            var instance = _epmConfig.GetSetting("Data:ActiveInstance");
            var result = _epmConfig.GetConnectionString($"EPMContext.{instance}");
			return result;
		}
	}
}
