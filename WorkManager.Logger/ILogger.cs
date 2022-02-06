using System;
using System.Runtime.CompilerServices;

namespace WorkManager.Logger
{
    public interface ILogger<T>
    {
        void Info(string message, [CallerMemberName] string callerMember = "");
        void Warning(string message, [CallerMemberName] string callerMember = "");
        void Error(string message, Exception ex, [CallerMemberName] string callerMember = "");
        void Log(LogType type, string message, Exception ex, [CallerMemberName] string callerMember = "");
    }
}