using System;
using System.Collections.Generic;
using System.Linq;
using WorkManager.Models.Interfaces;

namespace WorkManager.Extensions
{
    public static class ListExtension
    {
        public static void AddIfNotExists<T>(this IList<T> lst, T value) where T : IModel
        {
            if (lst == null)
                throw new ArgumentException("lst");
            if (lst.FirstOrDefault(s => s.Id == value.Id) == null)
                lst.Add(value);
        }

        public static IList<T> AddRange<T>(this IList<T> lst, IEnumerable<T> secondList)
        {
            if (lst == null)
                throw new ArgumentException("lst");
            if (secondList == null)
                return lst;
            foreach (T value in secondList)
            {
                lst.Add(value);
            }
            return lst;
        }

        public static void ForEach<T>(this IEnumerable<T> lst, Action<T> action)
        {
            if (lst == null)
                throw new ArgumentNullException("lst");
            if (action == null) 
                throw new ArgumentNullException("action");

            foreach (T item in lst)
            {
                action(item);
            }
        }

        public static IList<T> AddRange<T>(this IList<T> lst, params T[] secondList)
        {
            if (lst == null) 
                throw new ArgumentNullException("lst");
            return AddRange(lst, secondList.ToList());
        }

        public static IList<T> RemoveAll<T>(this IList<T> lst, Func<T, bool> predicate)
        {
            if (lst == null) 
                throw new ArgumentNullException("lst");
            return lst.RemoveRange(lst.Where(predicate));
        }

        public static IList<T> RemoveRange<T>(this IList<T> lst, IEnumerable<T> removeList)
        {
            if (lst == null) 
                throw new ArgumentNullException("lst");
            IList<T> clearedList = lst.ToList();
            removeList.ForEach(s => clearedList.Remove(s));
            return clearedList;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumeration)
        {
            return enumeration == null || !enumeration.Any();
        }
    }
}