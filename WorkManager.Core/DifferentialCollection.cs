using System.Collections.Generic;

namespace WorkManager.Core
{
	public class DifferentialCollection<T>
	{
		public IEnumerable<T> AddEnumerable { get; }
		public IEnumerable<T> DeleteEnumerable { get; }

		public DifferentialCollection(IEnumerable<T> addEnumerable, IEnumerable<T> deleteEnumerable)
		{
			AddEnumerable = addEnumerable;
			DeleteEnumerable = deleteEnumerable;
		}
	}
}