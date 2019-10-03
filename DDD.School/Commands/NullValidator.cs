using System.Threading;
using System.Threading.Tasks;

namespace DDD.School.Commands
{
    public sealed class NullValidator<TCommand> : IValidator<TCommand>
    {
        public Task<ValidationResult> ValidateAsync(TCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(ValidationResult.Successful);
        }
    }
}