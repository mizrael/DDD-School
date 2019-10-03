using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Persistence;

namespace DDD.School.Commands
{
    public class CreateStudent : MediatR.INotification
    {
        public CreateStudent(Guid studentId, string studentFirstname, string studentLastname)
        {
            if (Guid.Empty == studentId)
                throw new ArgumentOutOfRangeException(nameof(studentId));

            if (string.IsNullOrWhiteSpace(studentFirstname))
                throw new ArgumentNullException(nameof(studentFirstname));
            if (string.IsNullOrWhiteSpace(studentLastname))
                throw new ArgumentNullException(nameof(studentLastname));

            StudentId = studentId;
            StudentFirstname = studentFirstname;
            StudentLastname = studentLastname;
        }

        public Guid StudentId { get; }
        public string StudentFirstname { get; }
        public string StudentLastname { get; }
    }

    public class CreateStudentValidator : BaseValidator<CreateStudent>
    {
        private readonly ISchoolUnitOfWork _unitOfWork;

        public CreateStudentValidator(ISchoolUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task RunAsync(CreateStudent command, ValidationResult result, CancellationToken cancellationToken)
        {
            var courseById = await _unitOfWork.StudentsRepository.FindByIdAsync(command.StudentId, cancellationToken);
            if (null != courseById)
                result.AddError(nameof(CreateStudent.StudentId), $"there is already a student with id {command.StudentId}");
        }
    }

    public class CreateStudentHandler : BaseCommandHandler<CreateStudent>
    {
        private readonly ISchoolUnitOfWork _unitOfWork;
        public CreateStudentHandler(IValidator<CreateStudent> validator, ISchoolUnitOfWork unitOfWork) : base(validator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task RunCommand(CreateStudent command, CancellationToken cancellationToken)
        {
            var newCourse = new Student(command.StudentId, command.StudentFirstname, command.StudentLastname);
            await _unitOfWork.StudentsRepository.CreateAsync(newCourse, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
