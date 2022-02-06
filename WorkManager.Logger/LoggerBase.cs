using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WorkManager.DAL.Repositories;

namespace WorkManager.Logger
{
    public abstract class LoggerBase<T> : ILogger<T>
    {

        private readonly SystemRepository _systemRepository;

        protected LoggerBase(SystemRepository systemRepository)
        {
            _systemRepository = systemRepository;
        }

        public void Info(string message, [CallerMemberName] string callerMember = "")
        {
            InfoInt(CreateMessage(LogType.Info, message,null, callerMember));
        }

        public void Warning(string message, [CallerMemberName] string callerMember = "")
        {
            InfoInt(CreateMessage(LogType.Warning, message,null, callerMember));
        }

        public void Error(string message, Exception ex, [CallerMemberName] string callerMember = "")
        {
            InfoInt(CreateMessage(LogType.Error, message,null, callerMember));
        }

        public void Log(LogType type, string message, Exception ex, [CallerMemberName] string callerMember = "")
        {
            InfoInt(CreateMessage(type, message,null, callerMember));
        }

        private string CreateMessage(LogType type, string message, Exception? ex, string callerMember)
        {
            string exception = ex != null ? $"[{ex.Message}]" : string.Empty;
            return $"#{type} - [{_systemRepository.ActualServerTime():G}] [{typeof(T)}.{callerMember}] - {message} {exception}";
        }

        protected abstract void InfoInt(string message);
        protected abstract void WarningInt(string message);
        protected abstract void ErrorInt(string message);
    }
}