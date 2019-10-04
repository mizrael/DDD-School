namespace DDD.School.Persistence.SQL
{
    public interface IConnectionStringProvider
    {
        string ConnectionString { get; }
    }
}