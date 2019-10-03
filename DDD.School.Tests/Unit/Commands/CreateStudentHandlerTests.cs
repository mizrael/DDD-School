using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Commands;
using DDD.School.Persistence;
using NSubstitute;
using Xunit;

namespace DDD.School.Tests.Unit.Commands
{
    public class CreateStudentHandlerTests
    {
        [Fact]
        public async Task Handle_should_create_entity()
        {
            var repo = NSubstitute.Substitute.For<IStudentsRepository>();

            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.StudentsRepository.ReturnsForAnyArgs(repo);

            var validator = NSubstitute.Substitute.For<IValidator<CreateStudent>>();
            validator.ValidateAsync(null, CancellationToken.None)
                .ReturnsForAnyArgs(ValidationResult.Successful);

            var sut = new CreateStudentHandler(validator, unitOfWork);

            var command = new CreateStudent(Guid.NewGuid(), "new","student");
            await sut.Handle(command, CancellationToken.None);

            repo.Received(1).CreateAsync(Arg.Is((Student c) => c.Id == command.StudentId && c.Firstname == command.StudentFirstname && c.Lastname == command.StudentLastname), Arg.Any<CancellationToken>());
            unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }
    }
}