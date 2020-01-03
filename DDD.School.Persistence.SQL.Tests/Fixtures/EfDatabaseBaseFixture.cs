using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DDD.School.Persistence.SQL.Tests.Fixtures
{
    public abstract class EfDatabaseBaseFixture<TDb> : BaseFixture, IDisposable
        where TDb : DbContext
    {
        private readonly DbContextOptions<TDb> _options;
        
        public string ConnectionString { get; }

        protected EfDatabaseBaseFixture() 
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{base.EnvironmentName}.json", true)
                .Build();

            var section = config.GetSection("ConnectionStrings");
            if(null == section || !section.Exists())
                throw new KeyNotFoundException("missing ConnectionStrings section in config");

            var connectionString = section.GetChildren().FirstOrDefault(c => c.Key.StartsWith("Parametrized"));
            if(null == connectionString)
                throw new KeyNotFoundException("missing Parametrized connection string  in config");
            
            this.ConnectionString = string.Format(connectionString.Value, Guid.NewGuid());

            _options = new DbContextOptionsBuilder<TDb>()
                .UseSqlServer(this.ConnectionString)
                .EnableSensitiveDataLogging()
                .Options;
        }

        public TDb BuildDbContext()
        {
            try
            {
                var ctx = BuildDbContext(_options);
                ctx?.Database?.EnsureCreated();
                
                return ctx;
            }
            catch (Exception ex)
            {
                var builder = new SqlConnectionStringBuilder(this.ConnectionString);

                throw new Exception($"unable to connect to db {builder.InitialCatalog} on {builder.DataSource}", ex);
            }
        }

        protected abstract TDb BuildDbContext(DbContextOptions<TDb> options);

        public virtual void Dispose()
        {
            try
            {
                var dbCtx = BuildDbContext();
                dbCtx.Database.EnsureDeleted();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}