using DbSoft.Cache.Aspect;

namespace Dbsoft.Epm.Web.Infrastructure
{
    using DBSoft.EPM.DAL.Interfaces;

    public static class CacheConfig
    {
        public static void Configure()
        {
            CacheService.SessionProperty = "token";
            // CacheService.CacheProviderFactory = new RedisCacheProviderFactory(config.GetSetting("Cache:Hostname"), config.GetSetting("Cache:AccessKey"));
            CacheService.CacheProviderFactory = new CacheProviderFactory();
        }
    }
}
