namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using DTOs;
    using Enums;

    public interface IConfigurationService
	{
		ConfigurationSettingsDTO List(string token);

		T GetSetting<T>(string token, ConfigurationType type) where T : struct;

		void SaveSettings(string token, ConfigurationSettingsDTO settings);
        List<int> ListMarketSellLocations();
	}
}