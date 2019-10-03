namespace DDD.School.Persistence
{
    public interface ISchoolUnitOfWork : IUnitOfWork
    {
        ICoursesRepository CoursesRepository { get; }
        IStudentsRepository StudentsRepository { get; }
    }
}