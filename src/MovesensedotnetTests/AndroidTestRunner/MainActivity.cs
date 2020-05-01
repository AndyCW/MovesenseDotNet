using System;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;

using Xunit.Sdk;
using Xunit.Runners.UI;
using Android;
using Android.Content.PM;

namespace App1
{
    [Activity(Label = "xUnit Android Runner", MainLauncher = true, Theme = "@android:style/Theme.Material.Light")]
    public class MainActivity : RunnerActivity
    {
        readonly string[] PermissionsLocation =
{
          Manifest.Permission.AccessCoarseLocation
        };
        const int RequestLocationId = 0;

        protected override void OnCreate(Bundle bundle)
        {
            GetLocationPermissionAsync();

            // tests can be inside the main assembly
            AddTestAssembly(Assembly.GetExecutingAssembly());

            AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);
            // or in any reference assemblies			
            AddTestAssembly(typeof(MovesensedotNetTests.UnitTests).Assembly); 
            AddTestAssembly(typeof(MovesensedotNetConnectionTests.ConnectionUnitTests).Assembly);

            // or in any assembly that you load (since JIT is available)

#if false
            // you can use the default or set your own custom writer (e.g. save to web site and tweet it ;-)
            Writer = new TcpTextWriter ("10.0.1.2", 16384);
            // start running the test suites as soon as the application is loaded
            AutoStart = true;
            // crash the application (to ensure it's ended) and return to springboard
            TerminateAfterExecution = true;
#endif
            // you cannot add more assemblies once calling base
            base.OnCreate(bundle);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void GetLocationPermissionAsync()
        {
            //Check to see if any permission in our group is available, if one, then all are
            const string permission = Manifest.Permission.AccessCoarseLocation;
            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                return;
            }

            //Finally request permissions with the list of permissions and Id
            RequestPermissions(PermissionsLocation, RequestLocationId);
        }
    }

}

