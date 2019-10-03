using System.Threading;
using System.Threading.Tasks;

namespace DDD.School.Commands
{
    public abstract class BaseValidator<TCommand> : IValidator<TCommand>
    {
        public async Task<ValidationResult> ValidateAsync(TCommand command, CancellationToken cancellationToken)
        {
            var result = new ValidationResult();
            if (command == null)
                result.AddError(new ValidationError(nameof(command), "command cannot be null"));
            else
                await this.RunAsync(command, result, cancellationToken);
            return result;
        }

        protected abstract Task RunAsync(TCommand command, ValidationResult result, CancellationToken cancellationToken);
    }
}