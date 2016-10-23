namespace DBSoft.EPM.Logic
{
	using DAL.Interfaces;
    using System.Linq;
	using DAL.Services;
	using DAL.Services.AccountApi;

    public class ConfigurationProcessor : IConfigurationProcessor
    {
		private readonly IConfigurationService _config;
        private readonly IAccountApiService _accounts;

        public ConfigurationProcessor(IConfigurationService config, IAccountApiService accounts)
        {
            _config = config;
            _accounts = accounts;
        }

        public bool ConfigurationIsValid(string token)
		{
			var config = _config.List(token);
			return config.FactoryLocation > 0 &&
				config.MarketSellLocation > 0;
		}

		public bool AccountIsAvailable(string token)
		{
            return _accounts.List(token).Any();
		}
	}
}
