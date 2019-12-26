namespace DDD.School.Services
{

    public interface IEventSerializer
    {
        string Serialize<TE>(TE @event) where TE : IDomainEvent;
    }
}