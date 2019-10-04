using System;

namespace DDD.School.Persistence.SQL
{
    public class SchoolUnitOfWork : BaseUnitOfWork<SchoolDbContext>, ISchoolUnitOfWork 
    {
        public ICoursesRepository CoursesRepository { get; }
        public IStudentsRepository StudentsRepository { get; }

        public SchoolUnitOfWork(SchoolDbContext dbContext, ICoursesRepository coursesRepository, IStudentsRepository studentsRepository) : base(dbContext)
        {
            CoursesRepository = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
            StudentsRepository = studentsRepository ?? throw new ArgumentNullException(nameof(studentsRepository));
        }
    }
}