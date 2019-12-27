using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace DDD.School.Persistence.SQL
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly SchoolDbContext _dbContext;

        public MessagesRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IReadOnlyCollection<Message>> FetchUnprocessedAsync(int batchSize, CancellationToken cancellationToken)
        {
            var results = await _dbContext.Messages.Where(m => null == m.ProcessedAt)
                .OrderBy(m => m.CreatedAt)
                .Take(batchSize)
                .ToArrayAsync(cancellationToken);
            return results.ToImmutableArray();
        }
    }
}