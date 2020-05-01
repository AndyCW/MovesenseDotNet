using MdsLibrary.Model;
using Plugin.Movesense;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using Xunit;

namespace MovesensedotNetConnectionTests
{

    public class ConnectionUnitTests
    {
        IDisposable scan;
        public IAdapter BleAdapter => CrossBleAdapter.Current;
        private TaskCompletionSource<IDevice> bleconnectiontcs;


        [Fact]
        public async void ConnectDisconnectTest()
        {
            // Discover and connect to a device
            IDevice sensor = await ConnectToBLEAsync();

            Debug.WriteLine("Connecting to sensor");

            var movesense = Plugin.Movesense.CrossMovesense.Current;
            var movesenseDevice = await movesense.ConnectMdsAsync(sensor.Uuid);

            // Talk to the device
            //var info = await movesenseDevice.GetDeviceInfoAsync();

            // Disconnect Mds
            Debug.WriteLine("Disconnecting from sensor");
            await movesenseDevice.DisconnectMdsAsync();
        }

        private Task<IDevice> ConnectToBLEAsync()
        {
            bleconnectiontcs = new TaskCompletionSource<IDevice>();

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

            return bleconnectiontcs.Task;
        }

        private void DoScan()
        {
            scan = this.BleAdapter.Scan()
            .Subscribe(this.OnScanResult);
        }

        private void StopScanning()
        {
            this.scan?.Dispose();
        }

        void OnScanResult(IScanResult result)
        {
            // Only interested in Movesense devices
            if (result.Device.Name != null)
            {
                if (result.Device.Name.StartsWith("Movesense"))
                {
                    StopScanning();

                    var sensor = result.Device;

                    bleconnectiontcs?.TrySetResult(sensor);
                }
            }
        }
    }
}
