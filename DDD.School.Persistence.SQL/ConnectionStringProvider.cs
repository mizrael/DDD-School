using System;

namespace DDD.School.Persistence.SQL
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public ConnectionStringProvider(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}