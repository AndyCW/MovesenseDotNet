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
                    var info = await new MdsLibrary.Api.GetDeviceInfo(sensor.Name).CallAsync();
                    System.Diagnostics.Debug.WriteLine(info.GetContent().Serial);

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
