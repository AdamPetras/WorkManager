using System;
using System.Runtime.Serialization;

namespace WorkManager.BL.Exceptions
{
	public class UserNotExistsException : Exception
	{
		public UserNotExistsException()
		{
		}

		protected UserNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public UserNotExistsException(string message) : base(message)
		{
		}

		public UserNotExistsException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}