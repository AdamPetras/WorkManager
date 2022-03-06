using System;
using Xamarin.Forms;

namespace WorkManager.Xamarin.Core
{
    public class ClearEntryOnFocusIfTextEqualsTrigger : TriggerAction<Entry>
    {
        public string HintText { get; set; }

        public ClearEntryOnFocusIfTextEqualsTrigger()
        {
        }

        protected override void Invoke(Entry sender)
        {
            if (sender.Text == HintText)
            {
                sender.Text = null;
            }
        }
    }
}