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
    public partial class Loading : ContentView
    {
        public static readonly BindableProperty IsLoadingVisibleProperty = BindableProperty.Create(nameof(IsLoadingVisible), typeof(bool), typeof(Loading),default(bool), BindingMode.TwoWay, propertyChanged: IsLoadingVisibleChanged);

        public Loading()
        {
            InitializeComponent();
        }

        public bool IsLoadingVisible
        {
            get => (bool) GetValue(IsLoadingVisibleProperty);
            set => SetValue(IsLoadingVisibleProperty, value);
        }

        private static void IsLoadingVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((Loading)bindable).IsLoadingVisible = (bool)newvalue;
        }
    }
}