using System;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.Core.Annotations;

namespace WorkManager.Logger
{
    public class AppCenterLogger<T> : LoggerBase<T>
    {
        public AppCenterLogger(IServerCurrentTimeProvider serverCurrentTimeProvider) : base(serverCurrentTimeProvider)
        {
        }

        protected override void InfoInt(string message)
        {
            Analytics.TrackEvent(message);
        }

        protected override void WarningInt(string message)
        {
            Analytics.TrackEvent(message);
        }

        protected override void ErrorInt(string message)
        {
            Analytics.TrackEvent(message);
        }

        protected override void LogInt(string message)
        {
            Analytics.TrackEvent(message);
        }
    }
}