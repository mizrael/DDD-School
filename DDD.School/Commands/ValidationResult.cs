using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.School.Commands
{
    public class ValidationResult
    {
        public static readonly ValidationResult Successful = new ValidationResult(null);

        private readonly List<ValidationError> _errors;

        public ValidationResult(): this(null)
        {
        }

        public ValidationResult(IEnumerable<ValidationError> errors)
        {
            this._errors = (errors ?? Enumerable.Empty<ValidationError>()).ToList();
        }

        public IReadOnlyCollection<ValidationError> Errors => _errors.AsReadOnly();

        public void AddError(ValidationError error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            _errors.Add(error);
        }

        public void AddError(string field, string text, params object[] args)
        {
            string message = string.Format(text, args);
            this.AddError(new ValidationError(field, message));
        }

        public bool Success => !this.Errors.Any<ValidationError>();
    }
}