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
    public class CreateStudentValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_should_fail_when_Student_with_same_id_already_exists()
        {
            var student = new Student(Guid.NewGuid(), "existing", "Student");

            var repo = NSubstitute.Substitute.For<IStudentsRepository>();
            repo.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(student);

            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.StudentsRepository.ReturnsForAnyArgs(repo);
            var sut = new CreateStudentValidator(unitOfWork);

            var command = new CreateStudent(student.Id, "another", "Student");
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.Context == nameof(CreateStudent.StudentId) && e.Message.Contains(student.Id.ToString()));
        }

        [Fact]
        public async Task ValidateAsync_should_succeed_when_command_valid()
        {
            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            var sut = new CreateStudentValidator(unitOfWork);

            var command = new CreateStudent(Guid.NewGuid(), "new", "Student");
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}