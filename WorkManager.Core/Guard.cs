using System;

namespace WorkManager.Core
{
    public static class Guard
    {
        public static void ParameterNull(object value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }
    }
}