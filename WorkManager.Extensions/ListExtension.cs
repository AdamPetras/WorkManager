using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.Core;
using WorkManager.Models.Interfaces;

namespace WorkManager.Extensions
{
    public static class ListExtension
    {
        public static void AddIfNotExists<T>(this IList<T> list, T value) where T : IModel
        {
            Guard.ParameterNull(list, nameof(list));
            if (list.FirstOrDefault(s => s.Id == value.Id) == null)
                list.Add(value);
        }

        public static IList<T> AddRange<T>(this IList<T> list, IEnumerable<T> enumeration)
        {
            Guard.ParameterNull(list, nameof(list));
            if (enumeration == null)
                return list;
            foreach (T value in enumeration)
            {
                list.Add(value);
            }
            return list;
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            Guard.ParameterNull(enumeration, nameof(enumeration));
            Guard.ParameterNull(action, nameof(action));
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static async Task ForEachAwait<T>(this IEnumerable<T> enumeration, Func<T, CancellationToken, Task> action, CancellationToken token)
        {
            Guard.ParameterNull(enumeration, nameof(enumeration));
            Guard.ParameterNull(action, nameof(action));
            foreach (T item in enumeration)
            {
                await action(item, token);
            }
        }

        public static IList<T> AddRange<T>(this IList<T> list, params T[] secondList)
        {
            Guard.ParameterNull(list, nameof(list));
            return AddRange(list, secondList.ToList());
        }

        public static IList<T> RemoveAll<T>(this IList<T> list, Func<T, bool> predicate)
        {
            Guard.ParameterNull(list, nameof(list));
            return list.RemoveRange(list.Where(predicate));
        }

        public static IList<T> RemoveRange<T>(this IList<T> list, IEnumerable<T> removeList)
        {
            Guard.ParameterNull(list, nameof(list));
            Guard.ParameterNull(removeList, nameof(removeList));
            IList<T> clearedList = list.ToList();
            removeList.ForEach(s => clearedList.Remove(s));
            return clearedList;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumeration)
        {
            return enumeration == null || !enumeration.Any();
        }
    }
}