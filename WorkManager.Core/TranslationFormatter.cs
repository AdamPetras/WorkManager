using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WorkManager.Core
{
    public static class TranslationFormatter
    {
        public static string Format(this string value, params string[] parameters)
        {
            if (!Regex.IsMatch(value, "/*{[0-9]+}*"))
                throw new ArgumentException("Vstupní řetězec neobsahuje {X} kde X je číslo", nameof(value));
            for (var index = 0; index < parameters.Length; index++)
            {
                string parameter = parameters[index];
                value = value.Replace("{" + index + "}", parameter);
            }
            return value;
        }
    }
}