using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Persistence;

namespace DDD.School.Commands
{
    public class CreateCourse : MediatR.INotification
    {
        public CreateCourse(Guid courseId, string courseTitle)
        {
            if (Guid.Empty == courseId)
                throw new ArgumentOutOfRangeException(nameof(courseId));

            if (string.IsNullOrWhiteSpace(courseTitle))
                throw new ArgumentNullException(nameof(courseTitle));

            CourseId = courseId;
            CourseTitle = courseTitle;
        }

        public Guid CourseId { get; }
        public string CourseTitle { get; }
    }

    public class CreateCourseValidator : BaseValidator<CreateCourse>
    {
        private readonly ISchoolUnitOfWork _unitOfWork;

        public CreateCourseValidator(ISchoolUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task RunAsync(CreateCourse command, ValidationResult result, CancellationToken cancellationToken)
        {
            var courseById = await _unitOfWork.CoursesRepository.FindByIdAsync(command.CourseId, cancellationToken);
            if (null != courseById)
                result.AddError(nameof(CreateCourse.CourseId), $"there is already a course with id {command.CourseId}");

            var coursesByTitle = await _unitOfWork.CoursesRepository.FindAsync(
                u => u.Title == command.CourseTitle, cancellationToken);
            if (null != coursesByTitle && coursesByTitle.Any())
                result.AddError(nameof(CreateCourse.CourseTitle), $"there is already a course with title {command.CourseTitle}");
        }
    }

    public class CreateCourseHandler : BaseCommandHandler<CreateCourse>
    {
        private readonly ISchoolUnitOfWork _unitOfWork;
        public CreateCourseHandler(IValidator<CreateCourse> validator, ISchoolUnitOfWork unitOfWork) : base(validator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task RunCommand(CreateCourse command, CancellationToken cancellationToken)
        {
            var newCourse = new Course(command.CourseId, command.CourseTitle);
            await _unitOfWork.CoursesRepository.CreateAsync(newCourse, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
