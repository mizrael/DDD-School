using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.School.Persistence
{
    public interface IMessagesRepository
    {
        Task<IReadOnlyCollection<Message>> FetchUnprocessedAsync(int batchSize, CancellationToken cancellationToken);
    }
}