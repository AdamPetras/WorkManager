using System;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.Services;
using WorkManager.Logger;

namespace WorkManager.ViewModels.BaseClasses
{
    public class ViewModelTaskExecute : IDisposable
    {
        private readonly ViewModelCancellationTokensQueueService _viewModelCancellationTokensQueueService;
        private readonly ILogger<ViewModelCancellationTokensQueueService> _logger;

        public ViewModelTaskExecute(ViewModelCancellationTokensQueueService viewModelCancellationTokensQueueService, ILogger<ViewModelCancellationTokensQueueService> logger)
        {
            _viewModelCancellationTokensQueueService = viewModelCancellationTokensQueueService;
            _logger = logger;
        }

		public async Task<TRes> ExecuteTaskWithQueue<TRes>(Func<CancellationToken, Task<TRes>> action)
		{
			await _viewModelCancellationTokensQueueService.TerminateOldLoading();
			CancellationTokenSource cts = await _viewModelCancellationTokensQueueService.CreateTokenSource();
			try
			{
				return await action(cts.Token);
			}
			catch (Exception ex)
            {
                _logger.Error(string.Empty,ex);
                throw;
            }
			finally
			{
				cts.Dispose();
			}
		}

		public async Task ExecuteTaskWithQueue(Func<CancellationToken, Task> action)
		{
			await _viewModelCancellationTokensQueueService.TerminateOldLoading();
			CancellationTokenSource cts = await _viewModelCancellationTokensQueueService.CreateTokenSource();
			try
			{
				await action(cts.Token);
			}
			catch (Exception ex)
			{
                _logger.Error(string.Empty, ex);
                throw;
			}
			finally
			{
				cts.Dispose();
			}
		}

		public Task ExecuteTaskWithQueue<T>(T value, Func<T, CancellationToken, Task> action)
		{
			return ExecuteTaskWithQueue((t) => action(value, t));
		}

		public Task<T2> ExecuteTaskWithQueue<T, T2>(T value, Func<T, CancellationToken, Task<T2>> action)
		{
			return ExecuteTaskWithQueue((t) => action(value, t));
		}

		public Task<TRet> ExecuteTaskWithQueue<T1, T2, TRet>(T1 valueT1, T2 valueT2, Func<T1, T2, CancellationToken, Task<TRet>> action)
		{
			return ExecuteTaskWithQueue((t) => action(valueT1, valueT2, t));
		}

		public Task<TRet> ExecuteTaskWithQueue<T1, T2, T3, TRet>(T1 valueT1, T2 valueT2, T3 valueT3, Func<T1, T2, T3, CancellationToken, Task<TRet>> action)
		{
			return ExecuteTaskWithQueue((t) => action(valueT1, valueT2, valueT3, t));
		}

		public Task<TRet> ExecuteTaskWithQueue<T1, T2, T3, T4, TRet>(T1 valueT1, T2 valueT2, T3 valueT3, T4 valueT4, Func<T1, T2, T3, T4, CancellationToken, Task<TRet>> action)
		{
			return ExecuteTaskWithQueue((t) => action(valueT1, valueT2, valueT3, valueT4, t));
		}

		public Task<TRes> ExecuteTaskWithQueue<T1, TRes>(T1 valueT1, Func<CancellationToken, T1, Task<TRes>> action)
		{
			return ExecuteTaskWithQueue((t) => action(t, valueT1));
		}

		public Task<TRes> ExecuteTaskWithQueue<T1, T2, TRes>(T1 valueT1, T2 valueT2, Func<T1, CancellationToken, T2, Task<TRes>> action)
		{
			return ExecuteTaskWithQueue((t) => action(valueT1, t, valueT2));
		}

		public Task<TRes> ExecuteTaskWithQueue<T1, T2, T3, TRes>(T1 valueT1, T2 valueT2, T3 valueT3, Func<T1, T2, CancellationToken, T3, Task<TRes>> action)
		{
			return ExecuteTaskWithQueue((t) => action(valueT1, valueT2, t, valueT3));
		}

		public void Dispose()
		{
			_viewModelCancellationTokensQueueService?.Dispose();
		}
	}
}