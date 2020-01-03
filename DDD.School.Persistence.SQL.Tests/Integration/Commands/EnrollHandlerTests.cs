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
    public class EnrollHandlerTests : IClassFixture<SchoolDbContextFixture>
    {
        private readonly SchoolDbContextFixture _fixture;

        public EnrollHandlerTests(SchoolDbContextFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Handle_should_enroll_student()
        {
            await using var dbContext = _fixture.BuildDbContext();

            var student = new Student(Guid.NewGuid(), "student", "to enroll");
            dbContext.Students.Add(student);

            var course = new Course(Guid.NewGuid(), "course");
            dbContext.Courses.Add(course);

            await dbContext.SaveChangesAsync();
            //dbContext.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            //dbContext.Entry(course).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            var studentsRepo = new StudentsRepository(dbContext);
            var coursesRepo = new CoursesRepository(dbContext);
            var messagesRepository = NSubstitute.Substitute.For<IMessagesRepository>();
            var eventSerializer = NSubstitute.Substitute.For<IEventSerializer>();

            var unitOfWork = new SchoolUnitOfWork(dbContext, coursesRepo, studentsRepo, messagesRepository, eventSerializer);

            var sut = new EnrollHandler(new FakeValidator<Enroll>(), unitOfWork);

            var command = new Enroll(course.Id, student.Id);
            await sut.Handle(command, CancellationToken.None);

            var loadedStudent = await dbContext.Students.FindAsync(student.Id);
            loadedStudent.Courses.Count.Should().Be(1);
        }
    }
}