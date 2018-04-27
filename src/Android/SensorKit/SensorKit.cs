using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLStorage;
using System.IO;
using System.Collections.ObjectModel;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Threading;


namespace SensorKitSDK
{

    public class SensorKit : ViewModel
    {
        public static string folderPrefix = "sensorKit";

        private static SensorKit instance;
        public static SensorKit Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new SensorKit();
                }
                return instance;
            }
        }

        //private static AzureUploader uploaderInstance;
        //public static AzureUploader UploaderInstance
        //{
        //    get
        //    {
        //        if (uploaderInstance == null)
        //        {
        //            uploaderInstance = new AzureUploader();
        //        }
        //        return uploaderInstance;
        //    }
        //}

        public const string DEVICE_PREFIX = "SensorKit";
       
        private CancellationTokenSource _cancellationTokenSource;

        public bool IsCaptureOn { get; set; }

        public SensorsRegistry Registry { get; set; } = new SensorsRegistry();

        public ObservableCollection<SensorModel> Data { get; set; } = new ObservableCollection<SensorModel>();

        public IEnumerable<SensorModel> ExternalSensors => Data?.Where(s=>s.Id != default(Guid));

        public SensorModel LocalData { get; set; }

        public IBluetoothLE BLE { get; set; }
        public IAdapter Adapter { get; set; }

        private SensorKit()
        {

            // automatically add local model for the local on-phone sensors
            LocalData = new SensorModel()
            {
                Id = default(Guid),
                Name = SensorTypes.Hub.ToString(),
                Information = Registry.Items.FirstOrDefault(d => d.Model == SensorTypes.Hub)
            };
            Data.Add(LocalData);

            BLE = CrossBluetoothLE.Current;
            Adapter = CrossBluetoothLE.Current.Adapter;

            BLE.StateChanged += OnStateChanged;
            Adapter.DeviceDiscovered += OnDeviceDiscovered;
            Adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed;
            Adapter.DeviceDisconnected += OnDeviceDisconnected;
            Adapter.DeviceConnectionLost += OnDeviceConnectionLost;

        }

        public int Count
        {
            get
            {
                if (Data != null)
                    return Data.Count;
                else
                    return 0;
            }
        }

        public int CountSubscribed
        {
            get
            {
                if (Data != null)
                    return Data.Count(s=>s.IsSubscribed);
                else
                    return 0;
            }
        }
        
        

        private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            //Devices.FirstOrDefault(d => d.Id == e.Device.Id)?.Update();
        }

        private void OnStateChanged(object sender, BluetoothStateChangedArgs e)
        {
        }

        private void Adapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
            CleanupCancellationToken();
            isScanning = false;
        }

        private void CleanupCancellationToken()
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        private async void OnDeviceDiscovered(object sender, DeviceEventArgs args)
        {
            try
            {
                var name = args.Device.Name;

                //if (name != null && name.Contains(DEVICE_PREFIX) && !Data.Any(d => d.Name == name))
                if (name != null && Registry.Items.Any(r => name.ToLower().Contains(r.Format.ToLower())) && !Data.Any(d => d.Name == name))
                {
                    await AddSensor(args.Device);
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine(x);
            }
        }

        private void OnDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            //Devices.FirstOrDefault(d => d.Id == e.Device.Id)?.Update();
        }

        public void Start()
        {
            try
            {
                
                foreach (var model in Data)
                {
                    model.Start();
                }
                NotifyPropertyChanged("RecordingSequence");
            }
            catch (Exception x)
            {
                Debug.WriteLine(x);
            }
        }

        public void Stop()
        {
            try
            {
                foreach (var model in Data)
                {
                    model.Stop();
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine(x);
            }
        }


        bool isScanning = false;

        public async Task StartScanning()
        {
            if (!isScanning)
            {
                try
                {
                    isScanning = true;

                    // first check known devices
                    foreach (var connectedDevice in Adapter.ConnectedDevices)
                    {
                        var name = connectedDevice.Name;
                        //if (name != null && name.Contains(DEVICE_PREFIX) && !Data.Any(d => d.Name == name))
                        if (name != null && Registry.Items.Any(r => name.ToLower().Contains(r.Format.ToLower())) && !Data.Any(d => d.Name == name))
                        {
                            //update rssi for already connected evices (so that 0 is not shown in the list)
                            try
                            {
                                await connectedDevice.UpdateRssiAsync();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                            await AddSensor(connectedDevice);
                        }
                    }

                    _cancellationTokenSource = new CancellationTokenSource();
                    await Adapter.StartScanningForDevicesAsync(null, null, false, _cancellationTokenSource.Token);
                }
                catch (Exception x)
                {
                    Debug.WriteLine(x);
                }
            }
        }

        public async Task StopScanning()
        {
            try
            {
                if (isScanning)
                {
                    await Adapter.StopScanningForDevicesAsync();
                    isScanning = false;
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine(x);
            }
        }

        
        bool isAddingSensor = false;

        async Task AddSensor(IDevice device)
        {
            if (isAddingSensor)
                return;

            isAddingSensor = true;
            try
            {
                SensorModel sensor = null;
                sensor = await SensorKitExtensions.BeginInvokeOnMainThreadAsync<SensorModel>(()=>{
                    sensor = new SensorModel()
                    {
                        Id = device.Id,
                        Name = device.Name,
                        Information = Registry.Public.FirstOrDefault(s => s.Model == SensorInformation.TryParseSensorType(device.Name))
                    };
                    Data.Add(sensor);
                    return sensor;
                });
                
                if (sensor != null)
                {

                    InvokeHelper.Invoke(() =>
                    {
                        NotifyPropertyChanged("Data");
                    });
                }
            }catch(Exception x)
            {
                Debug.WriteLine(x);
            }
            isAddingSensor = false;
        }

        //public static async Task<bool> UploadAsync(string name, string path)
        //{
        //    try
        //    {
        //        return await UploaderInstance.UploadFileAsync(name, path);
        //    }
        //    catch(Exception x)
        //    {
        //        Debug.WriteLine(x);
        //    }
        //    return false;
        //}

        

    }
}
