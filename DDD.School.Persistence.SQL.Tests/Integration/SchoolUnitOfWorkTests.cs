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
    public class SchoolUnitOfWorkTests : IClassFixture<SchoolDbContextFixture>
    {
        private readonly SchoolDbContextFixture _fixture;
        public SchoolUnitOfWorkTests(SchoolDbContextFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CommitAsync_should_create_messages_from_domain_events()
        {
            await using var dbContext = _fixture.BuildDbContext();

            var studentsRepo = new StudentsRepository(dbContext);
            var coursesRepo = new CoursesRepository(dbContext);
            var messagesRepository = NSubstitute.Substitute.For<IMessagesRepository>();
            var eventSerializer = NSubstitute.Substitute.For<IEventSerializer>();

            var sut = new SchoolUnitOfWork(dbContext, coursesRepo, studentsRepo, messagesRepository, eventSerializer);

            var course = new Course(Guid.NewGuid(), "course");
            await sut.CoursesRepository.CreateAsync(course, CancellationToken.None);

            var student = new Student(Guid.NewGuid(), "firstname", "lastname");
            
            student.Enroll(course);
            student.Complete(course);

            await sut.StudentsRepository.CreateAsync(student, CancellationToken.None);

            await sut.CommitAsync(CancellationToken.None);

            var messages = await dbContext.Messages.ToListAsync();
            messages.Should().HaveCount(2);
        }
    }
}
