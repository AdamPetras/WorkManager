using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkManager.Core
{
	public class EnumerableDiffChecker<T>
	{
		public DifferentialCollection<T> CheckCollectionDifference(IEnumerable<T> initialEnumerable, IEnumerable<T> finalEnumerable)
		{
			return CheckCollectionDifference(initialEnumerable.ToList(), finalEnumerable.ToList());
		}

		public DifferentialCollection<T> CheckCollectionDifference(IEnumerable<T> initialEnumerable, IEnumerable<T> finalEnumerable, Func<T, T, bool> predicate)
		{
			return CheckCollectionDifference(initialEnumerable.ToList(), finalEnumerable.ToList(), predicate);
		}

		public DifferentialCollection<T> CheckCollectionDifference(ICollection<T> initialEnumerable, ICollection<T> finalEnumerable)
		{
			if (initialEnumerable == null || finalEnumerable == null)
				throw new ArgumentException();
			List<T> add = finalEnumerable.Where(a => initialEnumerable.All(b => !b.Equals(a))).ToList();
			List<T> delete = initialEnumerable.Where(a => finalEnumerable.All(b => !b.Equals(a))).ToList();
			return new DifferentialCollection<T>(add, delete);
		}

		public DifferentialCollection<T> CheckCollectionDifference(ICollection<T> initialEnumerable, ICollection<T> finalEnumerable, Func<T,T, bool> predicate)
		{
			if (initialEnumerable == null || finalEnumerable == null)
				throw new ArgumentException();
			List<T> add = finalEnumerable.Where(a => initialEnumerable.All(s=>!predicate(a, s))).ToList();
			List<T> delete = initialEnumerable.Where(a => finalEnumerable.All(s=>!predicate(a,s))).ToList();
			List<T> update = initialEnumerable.Where(a => finalEnumerable.Any(s=>predicate(a, s))).ToList();
			return new DifferentialCollection<T>(add, delete, update);
		}
	}
}