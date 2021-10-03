using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkManager.Views.Components
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SwitchText : ContentView
	{
		public SwitchText()
		{
			InitializeComponent();
		}

		//public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(SwitchText));
		//public object CommandParameter
		//{
		//	get => (object)GetValue(CommandParameterProperty);
		//	set => SetValue(CommandParameterProperty, value);
		//}

		//public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SwitchText), default(ICommand),BindingMode.OneWay,null, OnCommandChanged);

		//private static void OnCommandChanged(BindableObject bindable, object oldvalue, object newvalue)
		//{
		//	((SwitchText) bindable).Command = (ICommand)newvalue;
		//}

		//public ICommand Command
		//{
		//	get => (ICommand)GetValue(CommandProperty);
		//	set => SetValue(CommandProperty, value);
		//}

		public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(SwitchText), default(bool), BindingMode.TwoWay);

		public bool IsToggled
		{
			get => (bool)GetValue(IsToggledProperty);
			set
			{
				SetValue(IsToggledProperty, value);
				OnPropertyChanged(nameof(Text));
			}
		}

		public static readonly BindableProperty TextOnProperty = BindableProperty.Create(nameof(TextOn), typeof(string), typeof(SwitchText), default(string), BindingMode.OneWay,null, OnTextOnChanged);

		private static void OnTextOnChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((SwitchText) bindable).TextOn = (string)newvalue;
		}

		public string TextOn
		{
			get => (string)GetValue(TextOnProperty);
			set
			{
				SetValue(TextOnProperty, value);
				OnPropertyChanged(nameof(Text));
			}
		}

		public static readonly BindableProperty TextOffProperty = BindableProperty.Create(nameof(TextOff), typeof(string), typeof(SwitchText), default(string), BindingMode.OneWay,null, OnTextOffChanged);

		private static void OnTextOffChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((SwitchText) bindable).TextOff = (string)newvalue;
		}

		public string TextOff
		{
			get => (string)GetValue(TextOffProperty);
			set
			{
				SetValue(TextOffProperty, value);
				OnPropertyChanged(nameof(Text));
			}
		}

		public string Text
		{
			get
			{
				if (string.IsNullOrWhiteSpace(TextOn))
					return TextOff;
				if (string.IsNullOrWhiteSpace(TextOff))
					return TextOn;
				return IsToggled ? TextOff : TextOn;
			}
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			IsToggled = !IsToggled;
		}
	}
}