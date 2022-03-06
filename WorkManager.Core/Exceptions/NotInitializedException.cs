using System;
using System.Runtime.Serialization;
using WorkManager.Core.Annotations;

namespace WorkManager.Core.Exceptions
{
    public class NotInitializedException : Exception
    {
        public NotInitializedException()
        {
        }

        protected NotInitializedException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotInitializedException(string message) : base(message)
        {
        }

        public NotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}