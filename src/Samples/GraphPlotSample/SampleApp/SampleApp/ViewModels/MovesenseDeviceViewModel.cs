using GalaSoft.MvvmLight;
using MdsLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BluetoothLE;

namespace SampleApp.ViewModels
{
    public class MovesenseDeviceViewModel : ViewModelBase
    {
        public IDevice Device { get; private set; }

        public MovesenseDeviceViewModel()
        {
            IsSelected = false;
            DeviceStatus = DeviceStatus.Undefined;
        }

        Guid uuid;
        public Guid Uuid
        {
            get => this.uuid;
            private set => Set<Guid>(ref this.uuid, value);
        }

        string name;
        public string Name
        {
            get => this.name;
            private set => Set<string>(ref this.name, value);
        }

        public string MACAddress
        {
            get
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

        int rssi;
        public int Rssi
        {
            get => this.rssi;
            private set => this.Set<int>(ref this.rssi, value);
        }


        bool connectable;
        public bool IsConnectable
        {
            get => this.connectable;
            private set => this.Set<bool>(ref this.connectable, value);
        }


        int serviceCount;
        public int ServiceCount
        {
            get => this.serviceCount;
            private set => this.Set<int>(ref this.serviceCount, value);
        }


        string manufacturerData;
        public string ManufacturerData
        {
            get => this.manufacturerData;
            private set => this.Set<string>(ref this.manufacturerData, value);
        }


        string localName;
        public string LocalName
        {
            get => this.localName;
            private set => this.Set<string>(ref this.localName, value);
        }


        int txPower;
        public int TxPower
        {
            get => this.txPower;
            private set => this.Set<int>(ref this.txPower, value);
        }


        public bool TrySet(IScanResult result)
        {
            var response = false;

            if (this.Uuid == Guid.Empty)
            {
                this.Device = result.Device;
                this.Uuid = this.Device.Uuid;

                response = true;
            }

            try
            {
                if (this.Uuid == result.Device.Uuid)
                {
                    response = true;

                    this.Name = result.Device.Name;
                    this.Rssi = result.Rssi;

                    var ad = result.AdvertisementData;
                    this.ServiceCount = ad.ServiceUuids?.Length ?? 0;
                    this.IsConnectable = ad.IsConnectable;
                    this.LocalName = ad.LocalName;
                    this.TxPower = ad.TxPower;
                    this.ManufacturerData = ad.ManufacturerData == null
                        ? null
                        : BitConverter.ToString(ad.ManufacturerData);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return response;
        }

        private bool _isSelected;
        public bool IsSelected {
            get { return _isSelected; }
            set
            {
                Set(() => IsSelected, ref _isSelected, value);
                this.RaisePropertyChanged("ImageCheckBox");
            }
        }

        public string ImageCheckBox
        {
            get
            {
                if (IsSelected)
                    return "checked.png";
                else
                    return "unchecked.png";
            }
        }

        private int _batteryLevel;
        public int BatteryLevel { get { return _batteryLevel; } set { Set(() => BatteryLevel, ref _batteryLevel, value); } }

        private string _softwareVersion;
        public string SoftwareVersion { get { return _softwareVersion; } set { Set(() => SoftwareVersion, ref _softwareVersion, value); } }

        private DeviceStatus _deviceStatus;

        public DeviceStatus DeviceStatus
        {
            get { return _deviceStatus; }
            set { Set<DeviceStatus>(ref _deviceStatus, value); }
        }

        public async Task Connect()
        {
            DeviceStatus = DeviceStatus.Connecting;
            this.Device.Connect();
            Debug.WriteLine("Ble Connected!");

            // Now do the Mds connection
            await Plugin.Movesense.CrossMovesense.Current.ConnectMdsAsync(Uuid);

            DeviceStatus = DeviceStatus.Connected;
        }

        public async Task Disconnect()
        {
            DeviceStatus = DeviceStatus.Connecting;

            // Disconnect Mds
            await Plugin.Movesense.CrossMovesense.Current.DisconnectMdsAsync(Uuid);

            // Disconnect SensorKit
            this.Device.CancelConnection();

            Debug.WriteLine("Ble DisConnected!");

            DeviceStatus = DeviceStatus.Discovered;
        }
    }

    public enum DeviceStatus
    {
        Undefined,
        Discovered,
        Connecting,
        Connected,
        Inactive,
        Logging,
        Recording,
        RecordingDone,
        NotFound,
        Error
    }
}
