using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Commands;
using DDD.School.Persistence;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DDD.School.Tests.Unit.Commands
{
    public class CreateCourseValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_should_fail_when_course_with_same_id_already_exists()
        {
            var course = new Course(Guid.NewGuid(), "existing course");
            
            var repo = NSubstitute.Substitute.For<ICoursesRepository>();
            repo.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(course);

            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.CoursesRepository.ReturnsForAnyArgs(repo);
            var sut = new CreateCourseValidator(unitOfWork);

            var command = new CreateCourse(course.Id, "another course");
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.Context == nameof(CreateCourse.CourseId) && e.Message.Contains(course.Id.ToString()));
        }

        [Fact]
        public async Task ValidateAsync_should_fail_when_course_with_same_title_already_exists()
        {
            var course = new Course(Guid.NewGuid(), "existing course");

            var repo = NSubstitute.Substitute.For<ICoursesRepository>();
            repo.FindAsync(null, Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(new[]{course});

            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.CoursesRepository.ReturnsForAnyArgs(repo);
            var sut = new CreateCourseValidator(unitOfWork);

            var command = new CreateCourse(Guid.NewGuid(), course.Title);
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.Context == nameof(CreateCourse.CourseTitle) && e.Message.Contains(course.Title));
        }

        [Fact]
        public async Task ValidateAsync_should_succeed_when_command_valid()
        {
            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            var sut = new CreateCourseValidator(unitOfWork);

            var command = new CreateCourse(Guid.NewGuid(), "new Course");
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
