using System.Collections.Generic;

namespace DDD.School
{
    public interface IHasEvents
    {
        IReadOnlyCollection<IDomainEvent> Events { get; }

        void ClearEvents();
    }
}