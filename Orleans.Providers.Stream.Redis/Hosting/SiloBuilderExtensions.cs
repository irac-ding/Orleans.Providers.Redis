using System;
using Orleans.Streaming;

namespace Orleans.Hosting
{
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Configure silo to use redis queue persistent streams. 
        /// </summary>
        public static ISiloHostBuilder AddRedisStreams(this ISiloHostBuilder builder, string name, Action<SiloRedisStreamConfigurator> configure)
        {
            var streamConfigurator = new SiloRedisStreamConfigurator(
               name,
               configureServicesDelegate => builder.ConfigureServices(configureServicesDelegate),
               configureAppPartsDelegate => builder.ConfigureApplicationParts(configureAppPartsDelegate));
            configure?.Invoke(streamConfigurator);
            return builder;
        }
    }
}
