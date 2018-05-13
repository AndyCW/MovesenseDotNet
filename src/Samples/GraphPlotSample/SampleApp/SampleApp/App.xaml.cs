using SampleApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SampleApp
{
    public partial class App : Application
    {
        public static Locator Locator { get; } = new Locator();

        public App ()
		{
			InitializeComponent();

            var mainPage = new NavigationPage(new LinearAccelerationPage());

            MainPage = mainPage;
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
