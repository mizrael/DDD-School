using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DDD.School.Persistence.SQL
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly SchoolDbContext _dbContext;

        public StudentsRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<Student> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Students.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task CreateAsync(Student student, CancellationToken cancellationToken)
        {
            if (null == student)
                throw new ArgumentNullException(nameof(student));
            await _dbContext.Students.AddAsync(student, cancellationToken);
        }
    }
}