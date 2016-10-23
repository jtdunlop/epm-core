using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DBSoft.EPM.DAL.Annotations;
using DBSoft.EPM.DAL.Interfaces;

namespace DBSoft.EPM.DAL
{
	using System.Configuration;
	using Extensions;

    [UsedImplicitly]
    public class EpmConfig : IEpmConfig
    {
		public T GetSetting<T>(string key) where T : struct
		{
            var setting = GetSetting(key);
			return ConfigurationExtensions.TryParse<T>(setting);
		}

        public string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        public string GetBuildVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = string.Join(".", FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion.Split('.').ToList().Take(3));
            return version;
        }
    }
}
