﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Messaging;
using StackExchange.Redis;

namespace Orleans.Providers.Clustering.Redis
{
    public static class ConfigurationExtensions
    {
        public static ISiloHostBuilder UseRedisClustering(this ISiloHostBuilder builder, Action<RedisOptions> configuration)
        {
            return builder.ConfigureServices(services =>
            {
                var options = new RedisOptions();
                configuration?.Invoke(options);
                services.AddSingleton(options).AddRedis();
            });
        }

        public static ISiloHostBuilder UseRedisClustering(this ISiloHostBuilder builder, string redisConnectionString, int db = 0)
        {
            return builder.ConfigureServices(services => services
                .AddSingleton(new RedisOptions { Database = db, ConnectionString = redisConnectionString })
                .AddRedis());
        }

        public static IClientBuilder UseRedisClustering(this IClientBuilder builder, Action<RedisOptions> configuration)
        {
            builder.Configure<ClusterMembershipOptions>(x => x.ValidateInitialConnectivity = true);
            return builder.ConfigureServices(services =>
            {
                var options = new RedisOptions();
                configuration?.Invoke(options);

                services
                    .AddSingleton(options)
                    .AddSingleton(new GatewayOptions() { GatewayListRefreshPeriod = GatewayOptions.DEFAULT_GATEWAY_LIST_REFRESH_PERIOD })
                    .AddRedis()
                    .AddSingleton<IGatewayListProvider, RedisGatewayListProvider>();
            });
        }

        public static IClientBuilder UseRedisClustering(this IClientBuilder builder, string redisConnectionString, int db = 0)
        {
            builder.Configure<ClusterMembershipOptions>(x => x.ValidateInitialConnectivity = true);
            return builder.ConfigureServices(services => services
                .Configure<RedisOptions>(opt =>
                {
                    opt.ConnectionString = redisConnectionString;
                    opt.Database = db;
                })
                .AddSingleton(new GatewayOptions() { GatewayListRefreshPeriod = GatewayOptions.DEFAULT_GATEWAY_LIST_REFRESH_PERIOD })
                .AddRedis()
                .AddSingleton<IGatewayListProvider, RedisGatewayListProvider>());
        }

        private static IServiceCollection AddRedis(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionMultiplexer>(context =>
                ConnectionMultiplexer.Connect(context.GetService<RedisOptions>().ConnectionString))
                .AddSingleton<IMembershipTable, RedisMembershipTable>();
            return services;
        }
    }
}