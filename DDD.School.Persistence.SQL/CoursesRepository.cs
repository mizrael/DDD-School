using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DDD.School.Persistence.SQL
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly SchoolDbContext _dbContext;

        public CoursesRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<Course> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Courses
                            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Course>> FindAsync(Expression<Func<Course, bool>> query, CancellationToken cancellationToken)
        {
            return await _dbContext.Courses
                .Where(query)
                .ToArrayAsync(cancellationToken);
        }

        public async Task CreateAsync(Course course, CancellationToken cancellationToken)
        {
            if (null == course)
                throw new ArgumentNullException(nameof(course));
            await _dbContext.Courses.AddAsync(course, cancellationToken);
        }
    }
}