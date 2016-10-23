using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DBSoft.EPM.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using ConfigurationExtensions = DBSoft.EPM.DAL.Extensions.ConfigurationExtensions;

namespace DBSoft.EPM.DAL.Mvc6
{
    public class EpmConfig : IEpmConfig
    {
        private readonly IConfigurationRoot _config;

        public EpmConfig(IConfigurationRoot config)
        {
            _config = config;
        }

        public T GetSetting<T>(string key) where T : struct
        {
            var setting = GetSetting(key);
            return ConfigurationExtensions.TryParse<T>(setting);
        }

        public string GetSetting(string key)
        {
            return _config[key];
        }

        public string GetConnectionString(string key)
        {
            return _config[$"Data:{key}:ConnectionString"];
        }
    }
}
