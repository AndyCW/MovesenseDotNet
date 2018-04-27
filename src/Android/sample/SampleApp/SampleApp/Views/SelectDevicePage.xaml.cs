using SampleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectDevicePage : ContentPage
	{
        private SelectDevicePageViewModel VM;

        public SelectDevicePage ()
		{
			InitializeComponent ();
            BindingContext = VM = App.Locator.SelectDeviceVM;
        }

        protected override void OnAppearing()
        {
            VM.Init();
            VM.StartScanning();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            VM.StopScanning();
            base.OnDisappearing();
        }
    }
}