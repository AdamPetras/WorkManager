using WorkManager.BL.Interfaces.Services;

namespace WorkManager.BL.Services
{
	public class WorkManagerSettingsService
	{
		private readonly ISettingsServiceManager _settingsManager;

		public WorkManagerSettingsService(ISettingsServiceManager settingsManager)
		{
			_settingsManager = settingsManager;
		}

		public string Username
		{
			get => _settingsManager.GetValue<string>();
			set => _settingsManager.SetValue(value);
		}

		public string Password
		{
			get => _settingsManager.GetValue<string>();
			set => _settingsManager.SetValue(value);
		}

		public bool SaveCredentials
		{
			get => _settingsManager.GetValue<bool>();
			set => _settingsManager.SetValue(value);
		}
	}
}