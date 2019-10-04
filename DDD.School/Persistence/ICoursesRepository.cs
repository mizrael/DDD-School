using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.School.Persistence
{
    public interface ICoursesRepository
    {
        Task<Course> FindByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Course>> FindAsync(Expression<Func<Course, bool>> query, CancellationToken cancellationToken);
        Task CreateAsync(Course course, CancellationToken cancellationToken);
    }
}