using Plugin.BluetoothLE;
using Plugin.Movesense;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovesensedotNetTests
{
    public class MdsConnectionFixture : IAsyncLifetime
    {
        IDisposable scan;
        public IAdapter BleAdapter => CrossBleAdapter.Current;
        public IMovesenseDevice MovesenseDevice { get; private set; }
        private TaskCompletionSource<IMovesenseDevice> connectiontcs;

        public Task InitializeAsync()
        {
            connectiontcs = new TaskCompletionSource<IMovesenseDevice>();

            if (BleAdapter.Status == AdapterStatus.PoweredOn)
            {
                scan = this.BleAdapter.Scan().Subscribe(this.OnScanResult);
            }
            else
            {
                BleAdapter.WhenStatusChanged().Subscribe(status =>
                {
                    if (status == AdapterStatus.PoweredOn)
                    {
                        scan = this.BleAdapter.Scan().Subscribe(this.OnScanResult);
                    }
                });
            }

            return connectiontcs.Task;
        }

        async void OnScanResult(IScanResult result)
        {
            // Only interested in Movesense devices
            if (result.Device.Name != null)
            {
                if (result.Device.Name.StartsWith("Movesense"))
                {
                    // Stop scanning
                    this.scan?.Dispose();

                    // Now do the Mds connection
                    MovesenseDevice = await Plugin.Movesense.CrossMovesense.Current.ConnectMdsAsync(result.Device.Uuid);

                    connectiontcs?.TrySetResult(MovesenseDevice);
                }
            }
        }


        public async Task DisposeAsync()
        {
            // Disconnect Mds
            await MovesenseDevice.DisconnectMdsAsync();
        }
    }
}
