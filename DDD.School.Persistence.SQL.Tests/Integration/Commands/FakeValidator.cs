using System.Threading;
using System.Threading.Tasks;
using DDD.School.Commands;

namespace DDD.School.Persistence.SQL.Tests.Integration.Commands
{

    internal sealed class FakeValidator<TCommand> : IValidator<TCommand>
    {
        public Task<ValidationResult> ValidateAsync(TCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(ValidationResult.Successful);
        }
    }
}