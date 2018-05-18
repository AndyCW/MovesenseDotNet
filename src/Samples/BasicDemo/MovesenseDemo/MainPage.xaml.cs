using MdsLibrary;
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
            this.scan = this.BleAdapter
                .Scan()
                .Subscribe(this.OnScanResult);
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

                    // Bluetooth connect
                    var sensor = result.Device;
                    sensor.Connect();

                    // Now do the Mds connection
                    var mdsconnSvc = new MdsConnectionService();
                    await mdsconnSvc.ConnectMdsAsync(GetMACAddress(sensor.Uuid));

                    // Talk to the device
                    var movesense = Plugin.Movesense.CrossMovesense.Current;
                    var info = await movesense.GetDeviceInfoAsync(sensor.Name);
                    var batt = await movesense.GetBatteryLevelAsync(sensor.Name);

                    await DisplayAlert(
                        "Success", 
                        $"Communicated with device {sensor.Name}, firmware version is: {info.DeviceInfo.Sw}, battery: {batt.ChargePercent}", 
                        "OK");

                    // Disconnect Mds
                    await mdsconnSvc.DisconnectMds(GetMACAddress(sensor.Uuid));
                    //Disconnect Bluetooth
                    sensor.CancelConnection();
                }
            }
        }

        public string GetMACAddress(Guid Uuid)
        {
            string[] idParts = Uuid.ToString().Split(new char[] { '-' });
            string macAddress = idParts.Last().ToUpper();
            StringBuilder formattedMAC = new StringBuilder();
            for (int i = 0; i < macAddress.Length; i += 2)
            {
                if (i > 0) formattedMAC.Append(":");
                formattedMAC.Append(macAddress.Substring(i, 2));
            }

            return formattedMAC.ToString();
        }
    }
}
