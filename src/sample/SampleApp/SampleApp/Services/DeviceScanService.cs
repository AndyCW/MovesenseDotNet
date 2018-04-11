using MdsLibrary;
using SampleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApp.Services
{
    public class DeviceScanService : IDeviceScanService
    {
        public event EventHandler<MovesenseDeviceFoundArgs> MovesenseDeviceFound;

        public async void StartScanning()
        {
            System.Diagnostics.Debug.WriteLine($"DeviceScanService: Starting scanning");

            var data = SensorKitSDK.SensorKit.Instance.Data;

            foreach (var bleDevice in data)
            {
                if (bleDevice.Name.ToLower().StartsWith("movesense"))
                {
                    Debug.WriteLine($"Sensorkit Device Data Known device: {bleDevice.Name}");
                    // Tell observers
                    MovesenseDeviceFound?.Invoke(this, new MovesenseDeviceFoundArgs(bleDevice));
                }
            }

            // Hook events for newly found devices
            SensorKitSDK.SensorKit.Instance.Data.CollectionChanged += Data_CollectionChanged;

            // SensorKit BLE device access
            await SensorKitSDK.SensorKit.Instance.StartScanning();
        }

        private void Data_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems.Count > 0)
            {
                foreach (var item in e.NewItems)
                {
                    var bleDevice = item as SensorKitSDK.SensorModel;
                    if (bleDevice.Name.ToLower().StartsWith("movesense"))
                    {
                        // Trace it to debug output
                        System.Diagnostics.Debug.WriteLine($"DeviceScanService: Discovered Name:{bleDevice.Name} ID:{bleDevice.Id}");

                        // Tell observers
                        MovesenseDeviceFound?.Invoke(this, new MovesenseDeviceFoundArgs(bleDevice));
                    }
                }
            }
        }

        public async void StopScanning()
        {
            System.Diagnostics.Debug.WriteLine($"DeviceScanService: Stopping scanning");

            // Unook events for found devices
            SensorKitSDK.SensorKit.Instance.Data.CollectionChanged -= Data_CollectionChanged;

            // SensorKit BLE device access
            await SensorKitSDK.SensorKit.Instance.StopScanning();
        }
    }

    public class MovesenseDeviceFoundArgs : EventArgs
    {
        private SensorKitSDK.SensorModel bleDevice;

        public SensorKitSDK.SensorModel Device { get { return bleDevice; } }

        public MovesenseDeviceFoundArgs(SensorKitSDK.SensorModel device)
        {
            bleDevice = device;
        }
    }
}