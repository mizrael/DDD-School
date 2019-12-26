using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DDD.School.Persistence.SQL
{
    public abstract class BaseUnitOfWork<TDB> : IUnitOfWork
        where TDB : DbContext
    {
        protected readonly TDB DbContext;

        protected BaseUnitOfWork(TDB dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await BeforeCommitAsync(cancellationToken);

            await DbContext.SaveChangesAsync(cancellationToken);
        }

        protected abstract Task BeforeCommitAsync(CancellationToken cancellationToken);
        
        public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);
            return new EfTransaction(transaction);
        }
    }
}