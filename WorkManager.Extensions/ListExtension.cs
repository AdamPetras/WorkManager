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
			if(lst.FirstOrDefault(s=>s.Id == value.Id) == null)
				lst.Add(value);
		}

		public static IList<T> AddRange<T>(this IList<T> lst, IEnumerable<T> secondList)
		{
			if (lst == null)
				throw new ArgumentException();
			if (secondList == null)
				return lst;
			foreach (T value in secondList)
			{
				lst.Add(value);
			}
			return lst;
		}

		public static IList<T> AddRange<T>(this IList<T> lst, params T[] secondList)
		{
			return AddRange(lst, secondList.ToList());
		}
	}
}