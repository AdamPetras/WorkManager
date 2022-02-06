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
    public partial class Loading : Grid
    {
        public static readonly BindableProperty IsLoadingVisibleProperty =
            BindableProperty.Create(nameof(IsLoadingVisible), typeof(bool), typeof(Loading), default(bool),
                BindingMode.TwoWay, propertyChanged: IsLoadingVisibleChanged);

        public static readonly BindableProperty LoadingFontSizeProperty = BindableProperty.Create(
            nameof(LoadingFontSize), typeof(double), typeof(Loading), 14.0, BindingMode.TwoWay, propertyChanged: LoadingFontSizeChanged);

        public Loading()
        {
            InitializeComponent();
        }

        public bool IsLoadingVisible
        {
            get => (bool) GetValue(IsLoadingVisibleProperty);
            set => SetValue(IsLoadingVisibleProperty, value);
        }

        public double LoadingFontSize
        {
            get => (double)GetValue(LoadingFontSizeProperty);
            set => SetValue(LoadingFontSizeProperty, value);
        }

        private static void LoadingFontSizeChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((Loading)bindable).LoadingFontSize = (double)newvalue;
        }

        private static void IsLoadingVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((Loading)bindable).IsLoadingVisible = (bool)newvalue;
        }
    }
}