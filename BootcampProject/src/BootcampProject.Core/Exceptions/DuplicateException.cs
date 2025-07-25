using System;
using System.Runtime.Serialization;

namespace BootcampProject.Core.Exceptions
{
    [Serializable]
    public class DuplicateException : Exception
    {
        public DuplicateException()
        {
        }

        public DuplicateException(string message) : base(message)
        {
        }

        public DuplicateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DuplicateException(string entityName, string fieldName, object value) 
            : base($"Entity '{entityName}' with {fieldName} '{value}' already exists.")
        {
        }

        protected DuplicateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}