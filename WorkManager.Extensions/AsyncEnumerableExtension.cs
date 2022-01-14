using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace WorkManager.Extensions
{
    public static class AsyncEnumerableExtension
    {
        public static async Task<ObservableCollection<T>> ToObservableCollectionAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
        {
            ObservableCollection<T> results = new ObservableCollection<T>();
            await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
                results.Add(item);
            return results;
        }
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
        {
            List<T> results = new List<T>();
            await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
                results.Add(item);
            return results;
        }
    }
}