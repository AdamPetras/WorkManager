using WorkManager.BL.Interfaces.Services;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace WorkManager.BL.Services
{
	public class ToastMessageService:IToastMessageService
	{
		private readonly IMainThread _mainThread;

		public ToastMessageService(IMainThread mainThread)
		{
			_mainThread = mainThread;
		}

		public void LongAlert(string message)
		{
			
			_mainThread.InvokeOnMainThreadAsync(()=> DependencyService.Get<IMessage>().LongAlert(message));
		}

		public void ShortAlert(string message)
		{
			_mainThread.InvokeOnMainThreadAsync(()=> DependencyService.Get<IMessage>().ShortAlert(message));
		}
	}
}