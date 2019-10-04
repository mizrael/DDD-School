using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Commands;
using DDD.School.Persistence.SQL.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace DDD.School.Persistence.SQL.Tests.Integration.Commands
{
    public class CreateCourseTests : IClassFixture<SchoolDbContextFixture>
    {
        private readonly SchoolDbContextFixture _fixture;
        public CreateCourseTests(SchoolDbContextFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Handle_should_create_entity()
        {
            await using var dbContext = _fixture.BuildDbContext();

            var studentsRepository = new StudentsRepository(dbContext);
            var coursesRepo = new CoursesRepository(dbContext);

            var unitOfWork = new SchoolUnitOfWork(dbContext, coursesRepo, studentsRepository);

            var sut = new CreateCourseHandler(new NullValidator<CreateCourse>(), unitOfWork);

            var command = new CreateCourse(Guid.NewGuid(), "new course");
            await sut.Handle(command, CancellationToken.None);

            var createdCourse = await coursesRepo.FindByIdAsync(command.CourseId, CancellationToken.None);
            createdCourse.Should().NotBeNull();
            createdCourse.Id.Should().Be(command.CourseId);
            createdCourse.Title.Should().Be(command.CourseTitle);
        }
    }
}
