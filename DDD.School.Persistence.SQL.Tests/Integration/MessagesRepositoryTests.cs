using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Persistence.SQL.Tests.Fixtures;
using DDD.School.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace DDD.School.Persistence.SQL.Tests.Integration
{

    [Trait("Category", "Integration")]
    public class MessagesRepositoryTests : IClassFixture<SchoolDbContextFixture>
    {
        private readonly SchoolDbContextFixture _fixture;
        public MessagesRepositoryTests(SchoolDbContextFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task FetchUnprocessedAsync_should_return_unprocessed_messages_only()
        {
            var batchSize = 6;

            var publisher = Substitute.For<IMessagePublisher>();

            var messages = Enumerable.Repeat(1, batchSize)
                .Select(CreateFakeMessage)
                .ToArray();

            int index = 0;
            foreach (var message in messages)
            {
                if(0 == (index++ % 2))
                    await message.Process(publisher, CancellationToken.None);
            }

            await using var dbContext = _fixture.BuildDbContext();
            dbContext.Messages.AddRange(messages);
            await dbContext.SaveChangesAsync();

            var sut = new MessagesRepository(dbContext);

            var results = await sut.FetchUnprocessedAsync(batchSize, CancellationToken.None);
            results.Should().NotBeNullOrEmpty()
                .And.HaveCount(3);
        }

        private static Message CreateFakeMessage(int i)
        {
            var @event = new FakeEvent() { 
                Text = i.ToString()
            };
            var eventSerializer = Substitute.For<IEventSerializer>();
            return Message.FromDomainEvent(@event, eventSerializer);
        }
    }
}
