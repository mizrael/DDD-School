using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DDD.School
{

    public abstract class BaseEntity<TKey> : IHasEvents
    {
        protected BaseEntity()
        {
            _events = new List<IDomainEvent>();
        }

        private readonly IList<IDomainEvent> _events;

        public IReadOnlyCollection<IDomainEvent> Events => _events.ToImmutableArray();

        public void ClearEvents()
        {
            _events.Clear();
        }

        protected void AddEvent<TE>(TE @event) where TE:IDomainEvent
        {
            _events.Add(@event);
        }

        public TKey Id { get; protected set; }

        public override bool Equals(object obj)
        {
            var entity = obj as BaseEntity<TKey>;
            return entity != null &&
                   this.GetType() == entity.GetType() &&
                   EqualityComparer<TKey>.Default.Equals(Id, entity.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.GetType(), Id);
        }

        public static bool operator ==(BaseEntity<TKey> entity1, BaseEntity<TKey> entity2)
        {
            return EqualityComparer<BaseEntity<TKey>>.Default.Equals(entity1, entity2);
        }

        public static bool operator !=(BaseEntity<TKey> entity1, BaseEntity<TKey> entity2)
        {
            return !(entity1 == entity2);
        }
    }
}