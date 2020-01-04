using System.Collections.Generic;

namespace DDD.School
{
    public interface IAggregateRoot {
        IReadOnlyCollection<IDomainEvent> Events { get; }

        void ClearEvents();
    }
}