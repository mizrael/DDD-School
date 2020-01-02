using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Commands;
using DDD.School.Persistence.SQL.Tests.Fixtures;
using DDD.School.Services;
using FluentAssertions;
using Xunit;

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