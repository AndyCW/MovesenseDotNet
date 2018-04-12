# MovesenseDotNet
Preview of Movesense .NET SDK for Xamarin Android. Mds API near complete, but some functions still need wrapping. iOS not supported yet.

To build your own app:
  * download all the source in this repo
  * Create your Xamarin Android or Xamarin Forms project
  * Add reference to project MdsLibrary, and to SensorKit (this one not essential, the sample app uses the bluetooth connectivity and some viewmodel classes from this library)
  * **IMPORTANT:** the MdsLib.aar that is wrapped does not support 64 bit targets. Therefore, you *must* change the supported architectutes of your Xamarin Android project. To set this, got to Project Properties - Android Options, scroll down to the bottom and then click Advanced. In the Advanced Android Options window, click Supported Architectures and deselect **x86_64** and **arm64-v8a**
  * Also on the Advanced Android Options page, you also need to increase the **Java Max Heap Size** value. Suggest you set this to **1G**. If you do not set this, your compilation will probably fail with Memory Exceeded error.
  * For bluetooth connectivity, you will need to request the ACCESS_COARSE_LOCATION, ACCESS_FINE_LOCATION, BLUETOOTH and BLUETOOTH_ADMIN permissions. Check these on the Project Properties - Android manifest settings page. You will also need to request permissions from the user - see MainActivity.cs in the sample app for the code for this.
  * To use the Mds library, you must initialize it with the current Android Activity. See code in OnActivityCreated method in MainApplication.cs in the sample app for this. You will need to add a reference to the Plugin.CurrentActivity NuGet package to support this.
  * If you are using the Bluetooth Connectivity functionality in SensorKit as the sample app does, you must also add a reference to the Plugin.Ble NuGet package to your Xamarin Android project.

Using the API: Study the sample app for an example of how to subscribe to accelerometer data.
Example of calling an API:

```C#
bool? mCancelled = false;
var battstatus = await new GetBatteryLevel(mCancelled, item.Name).PerformWithRetryAsync();
var batteryLevel = battstatus.GetBatteryLevel();
 ```