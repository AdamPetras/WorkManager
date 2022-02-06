using System;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using WorkManager.Core.Annotations;
using WorkManager.DAL.Repositories;

namespace WorkManager.Logger
{
    public class AppCenterLogger<T> : LoggerBase<T>
    {
        public AppCenterLogger(SystemRepository systemRepository) : base(systemRepository)
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
    }
}