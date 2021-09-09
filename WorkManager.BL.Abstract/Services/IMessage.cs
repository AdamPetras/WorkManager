namespace WorkManager.BL.Interfaces.Services
{
	public interface IMessage
	{
		void LongAlert(string message);
		void ShortAlert(string message);
	}
}