using System;
using System.Collections.Generic;
using System.Text;
using Dister4Net.Exceptions;
using StackExchange.Redis;

namespace Dister4Net.Modules.Connectors
{
    public class RedisConnector : Module
    {
        public override int StartPriority => 2;
        ConnectionMultiplexer redis;
        string connectionString;
        public override string ModuleName => "Redis Connector";
        public RedisConnector(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void Start()
        {
            try
            {
                redis = ConnectionMultiplexer.Connect(connectionString);
            }
            catch (Exception ex)
            {
                throw new ConnectorException("Connection to redis server failed", ex);
            }
        }
        public IDatabase GetDatabase()
            => redis.GetDatabase();
        public ISubscriber GetSubscriber()
            => redis.GetSubscriber();
    }
}
