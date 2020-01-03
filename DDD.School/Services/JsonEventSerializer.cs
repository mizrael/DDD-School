using System;
using System.Text.Json;

namespace DDD.School.Services
{

    public class JsonEventSerializer : IEventSerializer
    {
        public string Serialize<TE>(TE @event) where TE : IDomainEvent
        {
            if (null == @event)
                throw new ArgumentNullException(nameof(@event));
            var eventType = @event.GetType();
            var result = JsonSerializer.Serialize(@event, eventType);
            return result;
        }
    }
}