using DDD.School.Persistence;
using DDD.School.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DDD.School.Tests.Unit.Services
{
    public class MessageProcessorTests
    {
        [Fact]
        public async Task ProcessMessagesAsync_should_process_messages()
        {
            var batchSize = 5;

            var messages = Enumerable.Repeat(1, batchSize)
                .Select(CreateFakeMessage)
                .ToArray();

            var messagesRepository = Substitute.For<IMessagesRepository>();
            messagesRepository.FetchUnprocessedAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(messages);
            
            var unitOfWork = Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.MessagesRepository.Returns(messagesRepository);

            var logger = Substitute.For<ILogger<MessageProcessor>>();
            var publisher = Substitute.For<IMessagePublisher>();

            var sut = new MessageProcessor(unitOfWork, publisher, logger);

            await sut.ProcessMessagesAsync(batchSize, CancellationToken.None);

            messages.All(m => m.ProcessedAt != null)
                .Should()
                .BeTrue();
        }

        private static Message CreateFakeMessage(int i)
        {
            var @event = new FakeEvent() { Text = i.ToString() };
            var eventSerializer = Substitute.For<IEventSerializer>();
            return Message.FromDomainEvent(@event, eventSerializer);
        }

        [Fact]
        public async Task ProcessMessagesAsync_should_process_all_messages_despite_exceptions()
        {
            var batchSize = 6;

            var messages = Enumerable.Repeat(1, batchSize)
                .Select(CreateFakeMessage)
                .ToList();

            var messagesRepository = Substitute.For<IMessagesRepository>();
            messagesRepository.FetchUnprocessedAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(messages);

            var unitOfWork = Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.MessagesRepository.Returns(messagesRepository);

            var logger = Substitute.For<ILogger<MessageProcessor>>();
            var publisher = Substitute.For<IMessagePublisher>();

            int callCount = 0;
            publisher.WhenForAnyArgs(p => p.PublishAsync(Arg.Any<Message>(), Arg.Any<CancellationToken>()))
                .Do(i => { if (callCount++ % 2 == 0) throw new Exception(); });

            var sut = new MessageProcessor(unitOfWork, publisher, logger);

            await sut.ProcessMessagesAsync(batchSize, CancellationToken.None);

            int messageIndex = 0;
            foreach(var message in messages)
            {
                if (messageIndex++ % 2 == 0)
                    message.ProcessedAt.Should().BeNull();
                else
                    message.ProcessedAt.Should().NotBeNull();
            }
        }
    }
}
