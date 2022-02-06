using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;

namespace WorkManager.BL.Services
{
    public class ViewModelCancellationTokensQueueService : IDisposable
    {
        private bool _disposed;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly Queue<CancellationTokenSource> _usedTokens = new Queue<CancellationTokenSource>();

        public async Task TerminateOldLoading()
        {
            await _semaphore.WaitAsync();
            try
            {
                // pokud probiha puvodni nacitani dat, prerusim jej
                while (_usedTokens.Count > 0)
                {
                    CancellationTokenSource storedToken = _usedTokens.Dequeue();

                    if (!storedToken.IsCancellationRequested)
                    {
                        // o dispose se postara finaly blok
                        try
                        {
                            storedToken.Cancel();
                        }
                        catch (ObjectDisposedException)
                        {
                            // tato vyjimka me nezajima, ocekavam ji
                            // je ji lepe vyvolat, nez resit problemy se synchronizaci
                            // podobne konstrukce se pouzivaji i ve zdrojacich .NET frameworku
                        }
                    }
                }

                _disposed = true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<CancellationTokenSource> CreateTokenSource()
        {
            await _semaphore.WaitAsync();
            try
            {
                _disposed = false;
                CancellationTokenSource token = new CancellationTokenSource();
                _usedTokens.Enqueue(token);

                return token;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            // musim zrusit vsechny tokeny
            TerminateOldLoading().SafeFireAndForget();
        }
    }
}