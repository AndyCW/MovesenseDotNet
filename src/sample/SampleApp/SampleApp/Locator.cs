/*
  In App.xaml:
  <Application.Resources>
      <vm:Locator xmlns:vm="clr-namespace:XamarinFormsMVVMLight"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using SampleApp.Services;
using SampleApp.ViewModels;
using CommonServiceLocator;

namespace SampleApp
{
    public class Locator
    {
        public const string LinearAccelerationTestPage = "LinearAccelerationTestPage";

        /// <summary>
        /// Register all the used ViewModels, Services et. al. witht the IoC Container
        /// </summary>
        public Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // ViewModels
            SimpleIoc.Default.Register<LinearAccelerationPageViewModel>();
            SimpleIoc.Default.Register<SelectDevicePageViewModel>();

            // Services
            SimpleIoc.Default.Register<IDeviceScanService, DeviceScanService>();
        }


        /// <summary>
        /// Gets the SelectSensor property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SelectDevicePageViewModel SelectDeviceVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SelectDevicePageViewModel>();
            }
        }

        /// <summary>
        /// Gets the LinearAccelerationTest property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public LinearAccelerationPageViewModel LinearAccelerationPageVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LinearAccelerationPageViewModel>();
            }
        }
    }
}