using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace WorkManager.Logger
{
    public interface ILogger<T>
    {
        void Info(string message, [CallerMemberName] string callerMember = "");
        //Task InfoAsync(string message, CancellationToken token, [CallerMemberName] string callerMember = "");
        void Warning(string message, [CallerMemberName] string callerMember = "");
        //Task WarningAsync(string message, CancellationToken token, [CallerMemberName] string callerMember = "");
        void Error(string message, Exception ex, [CallerMemberName] string callerMember = "");
        //Task ErrorAsync(string message, Exception ex, CancellationToken token, [CallerMemberName] string callerMember = "");
        void Log(LogType type, string message, Exception ex, [CallerMemberName] string callerMember = "");
        //Task LogAsync(LogType type, string message, Exception ex, CancellationToken token, [CallerMemberName] string callerMember = "");
    }
}