using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace WorkManager.Extensions
{
    public static class TypeExtension
    {
        public static StringLengthAttribute GetStringLengthAttribute(this Type type, string propertyName)
        {
            return (type.GetProperty(propertyName) ?? throw new ArgumentException($"Argument type je null nebo property {propertyName} pro typ {type} neexistuje."))
                .GetCustomAttributes<StringLengthAttribute>().SingleOrDefault()??throw new InvalidOperationException($"Property {propertyName} pro typ {type} neobsahuje atribut StringLengthAttribute.");
        }
        public static int GetStringMaxLength(this Type type, string propertyName)
        {
            return (type.GetProperty(propertyName) ?? throw new ArgumentException($"Argument type je null nebo property {propertyName} pro typ {type} neexistuje."))
                .GetCustomAttributes<StringLengthAttribute>().SingleOrDefault()?.MaximumLength ?? throw new InvalidOperationException($"Property {propertyName} pro typ {type} neobsahuje atribut StringLengthAttribute.");
        }
    }
}