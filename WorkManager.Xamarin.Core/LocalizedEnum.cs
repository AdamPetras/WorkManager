using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WorkManager.Core;

namespace WorkManager.Xamarin.Core
{
    public class LocalizedEnum
    {
        private readonly object _enumValue;
        private string _localizationCache;

        public LocalizedEnum(object enumValue)
        {
            if (!enumValue.GetType().IsEnum)
                throw new ArgumentException(nameof(enumValue));
            _enumValue = enumValue;
        }

        protected bool Equals(LocalizedEnum other)
        {
            return Equals(_enumValue, other._enumValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LocalizedEnum) obj);
        }

        public override int GetHashCode()
        {
            return (_enumValue != null ? _enumValue.GetHashCode() : 0);
        }

        public T GetValue<T>() where T : Enum
        {
            return (T) _enumValue;
        }

        public Enum GetValue(Type t)
        {
            if (t.IsEnum)
            {
                if (_enumValue.GetType() == t)
                {
                    return (Enum)_enumValue;
                }
                throw new ArgumentException($"Hodnota {_enumValue} není typu {t.Name}.");
            }
            throw new ArgumentException($"Typ {t.Name} není enum.");
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(_localizationCache))
            {
                Type enumType = _enumValue.GetType();
                string enumVal = Enum.GetName(enumType, _enumValue);
                _localizationCache =
                    enumType.GetField(enumVal).GetCustomAttributes(false).OfType<LocalizeAttribute>().SingleOrDefault()
                        ?.Description ?? $"[NO LOC ATT {enumVal}]";
            }
            return _localizationCache;
        }
    }
}