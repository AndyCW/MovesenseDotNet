using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovesenseDemo
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        IDisposable scan;
        public IAdapter BleAdapter => CrossBleAdapter.Current;

        private void OnClicked(object sender, EventArgs e)
        {
            
            CrossBleAdapter.Current.WhenStatusChanged().Subscribe(status =>
            {
                if (status == AdapterStatus.PoweredOn)
                {
                    scan = this.BleAdapter.Scan()
                    .Subscribe(this.OnScanResult);
                }
            });

            //this.scan = this.BleAdapter
                //.ScanWhenAdap()
                //.Subscribe(this.OnScanResult);
        }

        public void StopScanning()
        {
            this.scan?.Dispose();
        }

        async void OnScanResult(IScanResult result)
        {
            // Only interested in Movesense devices
            if (result.Device.Name != null)
            {
                if (result.Device.Name.StartsWith("Movesense"))
                {
                    StopScanning();

                    // Now do the Mds connection
                    var sensor = result.Device;
                    var movesense = Plugin.Movesense.CrossMovesense.Current;
                    await movesense.ConnectMdsAsync(sensor.Uuid);

                    // Talk to the device
                    var info = await movesense.GetDeviceInfoAsync(sensor.Name);
                    var batt = await movesense.GetBatteryLevelAsync(sensor.Name);

                    // Turn on the LED
                    await movesense.SetLedStateAsync(sensor.Name, 0, true);

                    await DisplayAlert(
                        "Success", 
                        $"Communicated with device {sensor.Name}, firmware version is: {info.DeviceInfo.Sw}, battery: {batt.ChargePercent}", 
                        "OK");

                    // Turn the LED off again
                    await movesense.SetLedStateAsync(sensor.Name, 0, false);

                    // Disconnect Mds
                    await movesense.DisconnectMdsAsync(sensor.Uuid);

                }
            }
        }
    }
}
