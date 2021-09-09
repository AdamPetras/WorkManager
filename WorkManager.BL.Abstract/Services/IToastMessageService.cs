namespace WorkManager.BL.Interfaces.Services
{
	public interface IToastMessageService
	{
		void LongAlert(string message);
		void ShortAlert(string message);
	}
}