using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BootcampProject.Core.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException() : base("One or more validation errors occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors) : this()
        {
            Errors = errors;
        }

        public ValidationException(string field, string error) : this()
        {
            Errors = new Dictionary<string, string[]>
            {
                { field, new[] { error } }
            };
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Errors = new Dictionary<string, string[]>();
        }
    }
}