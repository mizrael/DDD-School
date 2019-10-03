using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.School.Persistence
{
    public interface IStudentsRepository
    {
        Task<Student> FindByIdAsync(Guid id, CancellationToken cancellationToken);
        Task CreateAsync(Student student, CancellationToken cancellationToken);
    }
}