using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WorkManager.Core;

namespace WorkManager.Extensions
{
	public static class CollectionExtension
	{
		public static System.Collections.ObjectModel.ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableList)
		{
			return enumerableList != null ? new System.Collections.ObjectModel.ObservableCollection<T>(enumerableList) : null;
		}
	}
}