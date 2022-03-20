using System;
using System.Collections.Concurrent;
using System.Linq;
using WorkManager.BL.Interfaces.Services;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace WorkManager.BL.Services
{
    public class ToastMessageService:IToastMessageService
	{
        private class Message
        {
            public EMessageDuration Duration { get; }
            public string Text { get; }

            public Message(EMessageDuration duration, string text)
            {
                Duration = duration;
                Text = text;
            }
        }

        private enum EMessageDuration
        {
            Long,
            Short
        }

        private readonly IMainThread _mainThread;

        private ConcurrentQueue<Message> _messages;

		public ToastMessageService(IMainThread mainThread)
		{
			_mainThread = mainThread;
            _messages = new ConcurrentQueue<Message>();
        }

        private void ShowMessage()
        {
            if (!_messages.Any())
            {
                return;
            }

            _messages.TryDequeue(out Message message);
            switch (message.Duration)
            {
                case EMessageDuration.Long:
                        _mainThread.BeginInvokeOnMainThread(() => DependencyService.Get<IMessage>().LongAlert(message.Text));
                    break;
                case EMessageDuration.Short:
                        _mainThread.BeginInvokeOnMainThread(() => DependencyService.Get<IMessage>().ShortAlert(message.Text));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ShowMessage();  //zavolám rekurzivně pokud nejsou žádné zprávy dojde k vyskočení nahoře
        }

        public void LongAlert(string message)
		{
            _messages.Enqueue(new Message(EMessageDuration.Long, message));
            ShowMessage();
        }

		public void ShortAlert(string message)
		{
            _messages.Enqueue(new Message(EMessageDuration.Short, message));
            ShowMessage();
        }
	}
}