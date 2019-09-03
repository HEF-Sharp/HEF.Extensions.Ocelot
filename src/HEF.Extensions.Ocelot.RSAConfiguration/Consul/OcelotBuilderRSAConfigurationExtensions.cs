using CacheManager.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;

namespace HEF.Extensions.Ocelot.RSAConfiguration
{
    public static class OcelotBuilderRSAConfigurationExtensions
    {
        public static IOcelotBuilder AddConsulRSAConfiguration(this IOcelotBuilder builder)
        {
            var cacheManagerStringCache = CacheFactory.Build<string>("OcelotStringCache", x => x.WithDictionaryHandle());
            var ocelotStringCache = new OcelotCacheManagerCache<string>(cacheManagerStringCache);

            builder.Services.TryAddSingleton<IOcelotCache<string>>(ocelotStringCache);

            builder.Services.TryAddSingleton<IRSAConfigurationRepository, ConsulRSAConfigurationRepository>();
            
            return builder;
        }
    }
}
