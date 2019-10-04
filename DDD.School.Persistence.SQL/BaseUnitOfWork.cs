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

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }
        
        public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);
            return new EfTransaction(transaction);
        }
    }
}