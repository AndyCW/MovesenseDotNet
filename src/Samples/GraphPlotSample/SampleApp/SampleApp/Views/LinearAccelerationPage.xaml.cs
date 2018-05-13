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
	public partial class LinearAccelerationPage : ContentPage
	{
        private LinearAccelerationPageViewModel VM;

        public LinearAccelerationPage ()
		{
			InitializeComponent ();

            BindingContext = VM = App.Locator.LinearAccelerationPageVM;
        }
  
        private async void SelectDeviceButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectDevicePage());
        }

        protected override void OnAppearing()
        {
            VM.Init();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            VM.OnExit();
            base.OnDisappearing();
        }
    }
}