using MdsLibrary;
using Plugin.BluetoothLE;
using Plugin.Movesense;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomServiceSample
{
    public partial class MainPage : ContentPage
    {
        private IDevice mSelectedDevice;
        public ObservableCollection<Plugin.BluetoothLE.IDevice> MovesenseDevices = new ObservableCollection<Plugin.BluetoothLE.IDevice>();

        public MainPage()
        {
            InitializeComponent();
        }

        IDisposable scan;
        public IAdapter BleAdapter => CrossBleAdapter.Current;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            deviceListView.ItemsSource = MovesenseDevices;

            StatusLabel.Text = "No device selected";

            if (BleAdapter.Status == AdapterStatus.PoweredOn)
            {
                DoScan();
            }
            else
            {
                BleAdapter.WhenStatusChanged().Subscribe(status =>
                {
                    if (status == AdapterStatus.PoweredOn)
                    {
                        DoScan();
                    }
                });
            }
        }

        private void DoScan()
        {
            StatusLabel.Text = "Scanning for devices...";

            scan = this.BleAdapter.Scan()
            .Subscribe(this.OnScanResult);
        }

        private async void OnClicked(object sender, EventArgs e)
        {
            if (mSelectedDevice != null)
            {
                StopScanning();

                StatusLabel.Text = $"Connecting to device {mSelectedDevice.Name}";


                // Now do the Mds connection
                var movesense = Plugin.Movesense.CrossMovesense.Current;
                await movesense.ConnectMdsAsync(mSelectedDevice.Uuid);

                // Talk to the device
                // First get details of the app running on the device
                StatusLabel.Text = "Querying app details";

                var info = await movesense.GetAppInfoAsync(mSelectedDevice.Name);

                // Now try to get the HelloWorld resource
                try
                {
                    StatusLabel.Text = "Attempting to call HelloWorld service";

                    var helloWorldResponse = await movesense.ApiCallAsync<string>(mSelectedDevice.Name, Plugin.Movesense.Api.MdsOp.GET, "/Sample/HelloWorld");
                    await DisplayAlert(
                        "Success",
                        $"App on device {mSelectedDevice.Name} is {info.Data.ApplicationName} version {info.Data.ApplicationVersion}, GET of /Sample/HelloWorld returned {helloWorldResponse}",
                        "OK");
                }
                catch (MdsException ex)
                {
                    if (ex.Message.Contains("404"))
                    {
                        await DisplayAlert(
                            "HelloWorld not found",
                            $"App on device {mSelectedDevice.Name} is {info.Data.ApplicationName} version {info.Data.ApplicationVersion}, but GET of /Sample/HelloWorld failed because HelloWorld resource not found.",
                            "OK");
                    }
                }

                // Disconnect Mds
                await movesense.DisconnectMdsAsync(mSelectedDevice.Uuid);

                StatusLabel.Text = "Disconnected from device";

            }
        }

        public void StopScanning()
        {
            this.scan?.Dispose();
        }

        void OnScanResult(IScanResult result)
        {
            // Only interested in Movesense devices
            Plugin.BluetoothLE.IDevice device = result.Device;
            if (device.Name != null)
            {
                if (device.Name.StartsWith("Movesense"))
                {
                    if (MovesenseDevices.FirstOrDefault((d)=> d.Name == device.Name) == null)
                    {
                        MovesenseDevices.Add(device);
                        Debug.WriteLine($"Discovered device {device.Name}");
                    }
                }
            }
        }

        private void deviceListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            mSelectedDevice = (IDevice)e.SelectedItem;
        }
    }

}
