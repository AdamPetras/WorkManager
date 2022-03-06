using System;
using System.Runtime.Serialization;
using WorkManager.Core.Annotations;

namespace WorkManager.Core.Exceptions
{
    public class AlreadyInitializedException : Exception
    {
        public AlreadyInitializedException()
        {
        }

        protected AlreadyInitializedException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AlreadyInitializedException(string message) : base(message)
        {
        }

        public AlreadyInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}