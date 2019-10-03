using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace DDD.School.Commands
{
    public abstract class BaseCommandHandler<TCommand> : INotificationHandler<TCommand>
        where TCommand : INotification
    {
        private readonly IValidator<TCommand> _validator;

        protected BaseCommandHandler(IValidator<TCommand> validator)
        {
            this._validator = validator;
        }

        protected abstract Task RunCommand(TCommand command, CancellationToken cancellationToken);

        public async Task Handle(TCommand command, CancellationToken cancellationToken)
        {
            if ((object)command == null)
                throw new ArgumentNullException(nameof(command));
            if (this._validator != null)
            {
                ValidationResult validationResult = await this._validator.ValidateAsync(command, cancellationToken);
                if (validationResult == null)
                    throw new ValidationException("command validation failed");
                if (!validationResult.Success)
                    throw new ValidationException("command validation failed", validationResult.Errors);
            }
            await this.RunCommand(command, cancellationToken);
        }
    }
}