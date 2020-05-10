using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Plugin.Permissions;
using Xunit.Sdk;
using Xunit.Runners.UI;
using System.Reflection;

namespace AndroidTestRunner
{
    [Activity(Label = "xUnit Android Runner", MainLauncher = true, Theme = "@android:style/Theme.Material.Light")]
    public class MainActivity : RunnerActivity
    {
        readonly string[] PermissionsLocation =
        {
          Manifest.Permission.AccessCoarseLocation
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            CheckAndRequestLocationPermission();

            // tests can be inside the main assembly
            AddTestAssembly(Assembly.GetExecutingAssembly());

            AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);
            // or in any reference assemblies			

            AddTestAssembly(typeof(MovesensedotNetTests.UnitTests).Assembly);
            // or in any assembly that you load (since JIT is available)
            AddTestAssembly(typeof(MovesenseConnectionTests.ConnectionUnitTests).Assembly);

#if false
            // you can use the default or set your own custom writer (e.g. save to web site and tweet it ;-)
            Writer = new TcpTextWriter ("10.0.1.2", 16384);
            // start running the test suites as soon as the application is loaded
            AutoStart = true;
            // crash the application (to ensure it's ended) and return to springboard
            TerminateAfterExecution = true;
#endif
            // you cannot add more assemblies once calling base
            base.OnCreate(savedInstanceState);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            // Additionally could prompt the user to turn on in settings

            return status;
        }
    }
}