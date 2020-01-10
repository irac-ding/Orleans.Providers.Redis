﻿using System;
using Orleans.Providers.Common.Redis;
using Orleans.Runtime;

namespace Orleans.Configuration
{
    public class RedisStreamOptions : RedisOptions
    {
        /// <summary>
        /// Timeout period for Redis operations.
        /// </summary>
        public TimeSpan OperationTimeout { get; set; } = TimeSpan.FromSeconds(15);

        /// <summary>
        /// How many messages will be queued up before new messages
        /// are dropped on the floor. This queue is normally emptied during
        /// normal stream pulling.
        /// </summary>
        public int QueueCacheSize { get; set; } = 1000;

        public PersistenceLifetime PersistenceLifetime { get; set; } = DEFAULT_PERSISTENCE_LIFETIME;

        /// <summary>
        /// Dictates how stream pubsub channels are named. In nearly all normal situations this
        /// should probably be PersistenceLifetime.ClusterLifetime.
        /// </summary>
        public const PersistenceLifetime DEFAULT_PERSISTENCE_LIFETIME = PersistenceLifetime.ClusterLifetime;
    }

    public class RedisStreamOptionsValidator : IConfigurationValidator
    {
        private readonly RedisStreamOptions options;
        private readonly string name;

        public RedisStreamOptionsValidator(RedisStreamOptions options, string name)
        {
            this.options = options;
            this.name = name;
        }

        public void ValidateConfiguration()
        {
            if (String.IsNullOrEmpty(options.ConnectionString))
                throw new OrleansConfigurationException(
                    $"{nameof(RedisStreamOptions)} on stream provider {this.name} is invalid. {nameof(RedisStreamOptions.ConnectionString)} is invalid");
        }
    }
}