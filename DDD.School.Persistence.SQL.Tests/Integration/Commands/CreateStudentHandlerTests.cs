using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Commands;
using DDD.School.Persistence.SQL.Tests.Fixtures;
using DDD.School.Services;
using FluentAssertions;
using Xunit;

namespace DDD.School.Persistence.SQL.Tests.Integration.Commands
{
    [Trait("Category", "Integration")]
    public class CreateStudentHandlerTests : IClassFixture<SchoolDbContextFixture>
    {
        private readonly SchoolDbContextFixture _fixture;
        public CreateStudentHandlerTests(SchoolDbContextFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Handle_should_create_entity()
        {
            await using var dbContext = _fixture.BuildDbContext();

            var studentsRepo = new StudentsRepository(dbContext);
            var coursesRepo = new CoursesRepository(dbContext);
            var messagesRepository = NSubstitute.Substitute.For<IMessagesRepository>();
            var eventSerializer = NSubstitute.Substitute.For<IEventSerializer>();

            var unitOfWork = new SchoolUnitOfWork(dbContext, coursesRepo, studentsRepo, messagesRepository, eventSerializer);

            var sut = new CreateStudentHandler(new FakeValidator<CreateStudent>(), unitOfWork);

            var command = new CreateStudent(Guid.NewGuid(), "new", "student");
            await sut.Handle(command, CancellationToken.None);

            var createdStudent = await studentsRepo.FindByIdAsync(command.StudentId, CancellationToken.None);
            createdStudent.Should().NotBeNull();
            createdStudent.Id.Should().Be(command.StudentId);
            createdStudent.Firstname.Should().Be(command.StudentFirstname);
            createdStudent.Lastname.Should().Be(command.StudentLastname);
        }
    }
}