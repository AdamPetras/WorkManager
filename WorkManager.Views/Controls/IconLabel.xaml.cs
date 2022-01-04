using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkManager.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IconLabel : ContentView
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(IconLabel), default(string), BindingMode.TwoWay, propertyChanged: OnTextChanged);
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(FontImageSource), typeof(IconLabel), default(FontImageSource), BindingMode.TwoWay, propertyChanged: OnImageSourceChanged);
        public static readonly BindableProperty IconHeightProperty = BindableProperty.Create(nameof(IconHeight), typeof(int), typeof(IconLabel), default(int), BindingMode.TwoWay, propertyChanged: OnIconHeightChanged);

        public IconLabel()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public FontImageSource ImageSource
        {
            get => (FontImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public int IconHeight
        {
            get => (int)GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }

        private static void OnTextChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((IconLabel)bindable).Text = (string)newvalue;
        }

        private static void OnImageSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((IconLabel)bindable).ImageSource = (FontImageSource)newvalue;
        }

        private static void OnIconHeightChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((IconLabel)bindable).IconHeight = (int)newvalue;
        }
    }
}