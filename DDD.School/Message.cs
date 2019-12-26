using DDD.School.Services;
using System;

namespace DDD.School
{
    public class Message : BaseEntity<Guid>
    {
        private Message(Guid id, DateTime createdAt, string type, string payload) 
        {
            this.Id = id;
            this.CreatedAt = createdAt;
            this.Type = type;
            this.Payload = payload;
        }

        public DateTime CreatedAt { get; }
        public DateTime? ProcessedAt { get; }
        public string Type { get; }
        public string Payload { get; }

        public static Message FromDomainEvent<TE>(TE @event, IEventSerializer serializer) where TE : IDomainEvent
        {
            if (null == @event)
                throw new ArgumentNullException(nameof(@event));
            if (null == serializer)
                throw new ArgumentNullException(nameof(serializer));

            var type = @event.GetType().FullName;

            return new Message(Guid.NewGuid(), DateTime.UtcNow, type, serializer.Serialize(@event));
        }
    }
}