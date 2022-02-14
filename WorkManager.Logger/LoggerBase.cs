using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.DAL.Repositories;

namespace WorkManager.Logger
{
    public abstract class LoggerBase<T> : ILogger<T>
    {

        private readonly IServerCurrentTimeProvider _serverCurrentTimeProvider;

        protected LoggerBase(IServerCurrentTimeProvider serverCurrentTimeProvider)
        {
            _serverCurrentTimeProvider = serverCurrentTimeProvider;
        }

        public void Info(string message, [CallerMemberName] string callerMember = "")
        {
            InfoInt(CreateMessage(LogType.Info, message,null, callerMember));
        }

        //public async Task InfoAsync(string message, CancellationToken token, string callerMember = "")
        //{
        //    await InfoAsyncInt(await CreateMessageAsync(LogType.Info, message, null, callerMember, token), token);
        //}

        public void Warning(string message, [CallerMemberName] string callerMember = "")
        {
            WarningInt(CreateMessage(LogType.Warning, message,null, callerMember));
        }

        //public async Task WarningAsync(string message, CancellationToken token, string callerMember = "")
        //{
        //    await WarningAsyncInt(await CreateMessageAsync(LogType.Info, message, null, callerMember, token), token);
        //}


        public void Error(string message, Exception ex, [CallerMemberName] string callerMember = "")
        {
            ErrorInt(CreateMessage(LogType.Error, message,null, callerMember));
        }

        //public async Task ErrorAsync(string message, Exception ex, CancellationToken token, string callerMember = "")
        //{
        //    await ErrorAsyncInt(await CreateMessageAsync(LogType.Info, message, null, callerMember, token), token);
        //}

        public void Log(LogType type, string message, Exception ex, [CallerMemberName] string callerMember = "")
        {
            LogInt(CreateMessage(type, message,null, callerMember));
        }

        //public async Task LogAsync(LogType type, string message, Exception ex, CancellationToken token, string callerMember = "")
        //{
        //    await LogAsyncInt(await CreateMessageAsync(LogType.Info, message, null, callerMember, token), token);
        //}

        private string CreateMessage(LogType type, string message, Exception? ex, string callerMember)
        {
            string exception = ex != null ? $"[{ex.Message}]" : string.Empty;
            return $"#{type} - [{_serverCurrentTimeProvider.GetTime():G}] [{typeof(T)}.{callerMember}] - {message} {exception}";
        }

        //private async Task<string> CreateMessageAsync(LogType type, string message, Exception? ex, string callerMember, CancellationToken token)
        //{
        //    string exception = ex != null ? $"[{ex.Message}]" : string.Empty;
        //    return $"#{type} - [{await _serverCurrentTimeProvider.GetTimeAsync(token):G}] [{typeof(T)}.{callerMember}] - {message} {exception}";
        //}

        protected abstract void InfoInt(string message);
        //protected abstract Task InfoAsyncInt(string message, CancellationToken token);
        protected abstract void WarningInt(string message);
        //protected abstract Task WarningAsyncInt(string message, CancellationToken token);
        protected abstract void ErrorInt(string message);
        //protected abstract Task ErrorAsyncInt(string message, CancellationToken token);
        protected abstract void LogInt(string message);
        //protected abstract Task LogAsyncInt(string message, CancellationToken token);
    }
}