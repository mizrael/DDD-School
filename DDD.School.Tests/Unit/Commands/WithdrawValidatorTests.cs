using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Commands;
using DDD.School.Persistence;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DDD.School.Tests.Unit.Commands
{
    public class WithdrawValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_should_fail_when_Student_does_not_exists()
        {
            var repo = NSubstitute.Substitute.For<IStudentsRepository>();
            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.StudentsRepository.ReturnsForAnyArgs(repo);
            var sut = new WithdrawValidator(unitOfWork);

            var command = new Withdraw(Guid.NewGuid(), Guid.NewGuid());
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.Context == nameof(Withdraw.StudentId) && e.Message.Contains(command.StudentId.ToString()));
        }

        [Fact]
        public async Task ValidateAsync_should_fail_when_Course_does_not_exists()
        {
            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            var sut = new WithdrawValidator(unitOfWork);

            var command = new Withdraw(Guid.NewGuid(), Guid.NewGuid());
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.Context == nameof(Withdraw.CourseId) && e.Message.Contains(command.CourseId.ToString()));
        }

        [Fact]
        public async Task ValidateAsync_should_succeed_when_command_valid()
        {
            var student = new Student(Guid.NewGuid(), "existing", "Student");

            var studentsRepository = NSubstitute.Substitute.For<IStudentsRepository>();
            studentsRepository.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(student);

            var course = new Course(Guid.NewGuid(), "existing course");

            var coursesRepository = NSubstitute.Substitute.For<ICoursesRepository>();
            coursesRepository.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(course);

            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.StudentsRepository.ReturnsForAnyArgs(studentsRepository);
            unitOfWork.CoursesRepository.ReturnsForAnyArgs(coursesRepository);
            var sut = new WithdrawValidator(unitOfWork);

            var command = new Withdraw(course.Id, student.Id);
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
