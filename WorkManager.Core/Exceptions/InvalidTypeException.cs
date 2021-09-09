using System;
using System.Runtime.Serialization;

namespace WorkManager.Core.Exceptions
{
	public class InvalidTypeException : Exception
	{
		public InvalidTypeException(string valueName, Type actualType) : base(ExceptionsSR.InvalidTypeExceptionFormat(valueName, actualType))
		{
		}

		public InvalidTypeException(string valueName, Type actualType, Exception innerException) : base(ExceptionsSR.InvalidTypeExceptionFormat(valueName, actualType), innerException)
		{
		}
	}
}