using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Persistence;

namespace DDD.School.Commands
{
    public class Withdraw : MediatR.INotification
    {
        public Withdraw(Guid courseId, Guid studentId)
        {
            if (Guid.Empty == courseId)
                throw new ArgumentOutOfRangeException(nameof(courseId));
            if (Guid.Empty == studentId)
                throw new ArgumentOutOfRangeException(nameof(studentId));
            CourseId = courseId;
            StudentId = studentId;
        }

        public Guid CourseId { get; }
        public Guid StudentId { get; }
    }

    public class WithdrawValidator : BaseValidator<Withdraw>
    {
        private readonly ISchoolUnitOfWork _unitOfWork;

        public WithdrawValidator(ISchoolUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task RunAsync(Withdraw command, ValidationResult result, CancellationToken cancellationToken)
        {
            var courseById = await _unitOfWork.CoursesRepository.FindByIdAsync(command.CourseId, cancellationToken);
            if (null == courseById)
                result.AddError(nameof(Withdraw.CourseId), $"invalid course id {command.CourseId}");

            var studentById = await _unitOfWork.StudentsRepository.FindByIdAsync(command.StudentId, cancellationToken);
            if (null == studentById)
                result.AddError(nameof(Withdraw.StudentId), $"invalid student id {command.StudentId}");
        }
    }

    public class WithdrawHandler : BaseCommandHandler<Withdraw>
    {
        private readonly ISchoolUnitOfWork _unitOfWork;
        public WithdrawHandler(IValidator<Withdraw> validator, ISchoolUnitOfWork unitOfWork) : base(validator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task RunCommand(Withdraw command, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.CoursesRepository.FindByIdAsync(command.CourseId, cancellationToken); 
            var student = await _unitOfWork.StudentsRepository.FindByIdAsync(command.StudentId, cancellationToken);
            student.Withdraw(course);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}