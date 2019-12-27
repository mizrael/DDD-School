using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DDD.School.Tests.Unit
{

    public class MessageTests
    {
        [Fact]
        public void FromDomainEvent_should_create_message()
        {
            var @event = new FakeEvent() { Text = Guid.NewGuid().ToString() };

            var eventSerializer = Substitute.For<IEventSerializer>();
            var expectedPayload = $"{@event.Text}";
            eventSerializer.Serialize(@event)
                .Returns(expectedPayload);

            var sut = Message.FromDomainEvent(@event, eventSerializer);
            sut.Should().NotBeNull();
            sut.Payload.Should().Be(expectedPayload);
            sut.ProcessedAt.Should().BeNull();
            sut.Type.Should().Be(typeof(FakeEvent).FullName);
        }

        [Fact]
        public async Task Process_should_publish_message()
        {
            var @event = new FakeEvent() { Text = Guid.NewGuid().ToString() };

            var eventSerializer = Substitute.For<IEventSerializer>();
            
            var sut = Message.FromDomainEvent(@event, eventSerializer);

            sut.ProcessedAt.Should().BeNull();

            var publisher = Substitute.For<IMessagePublisher>();

            await sut.Process(publisher, CancellationToken.None);

            publisher.Received(1).PublishAsync(sut, CancellationToken.None);

            sut.ProcessedAt.Should().NotBeNull();
        }
    }

    internal class FakeEvent : IDomainEvent {
        public string Text;
    }
}