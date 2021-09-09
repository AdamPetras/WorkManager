using System;
using System.Runtime.Serialization;

namespace WorkManager.BL.Exceptions
{
	[Serializable]
	public class UserAlreadyExistsException : Exception
	{
		public UserAlreadyExistsException()
		{
		}

		protected UserAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public UserAlreadyExistsException(string message) : base(message)
		{
		}

		public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}