using System;
using System.Runtime.Serialization;
using WorkManager.Core.Annotations;

namespace WorkManager.BL.Exceptions
{
    public class PasswordStructureException : Exception
    {
        public PasswordStructureException()
        {
        }

        protected PasswordStructureException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public PasswordStructureException(string message) : base(message)
        {
        }

        public PasswordStructureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}