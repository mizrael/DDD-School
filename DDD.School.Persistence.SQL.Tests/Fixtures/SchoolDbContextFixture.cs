using Microsoft.EntityFrameworkCore;

namespace DDD.School.Persistence.SQL.Tests.Fixtures
{
    public class SchoolDbContextFixture : EfDatabaseBaseFixture<SchoolDbContext>
    {
        protected override SchoolDbContext BuildDbContext(DbContextOptions<SchoolDbContext> options)
        {
            return new SchoolDbContext(options);
        }
    }
}
