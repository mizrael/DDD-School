using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.School.Commands
{
    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : this(message, null)
        {
        }

        public ValidationException(string message, IEnumerable<ValidationError> errors)
            : this(message, errors, null)
        {
        }

        public ValidationException(string message, IEnumerable<ValidationError> errors, Exception innerEx)
            : base(message, innerEx)
        {
            this.Errors = errors ?? Enumerable.Empty<ValidationError>();
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}