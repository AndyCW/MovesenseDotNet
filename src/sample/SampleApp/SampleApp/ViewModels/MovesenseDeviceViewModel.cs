using GalaSoft.MvvmLight;
using SensorKitSDK;
using MdsLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.ViewModels
{
    public class MovesenseDeviceViewModel : ViewModelBase
    {
        public SensorModel SensorModel { get; set; }

        public MovesenseDeviceViewModel(Guid ID, string Name)
        {
            SensorModel = new SensorModel();
            SensorModel.Id = ID;
            SensorModel.Name = Name;
            IsSelected = false;
            DeviceStatus = DeviceStatus.Undefined;
        }

        public MovesenseDeviceViewModel(SensorModel sensorModel)
        {
            SensorModel = sensorModel;
            IsSelected = false;
            DeviceStatus = DeviceStatus.Discovered;
        }

        public Guid ID
        {
            get
            {
                return SensorModel.Id;
            }
        }

        public string Name
        {
            get
            {
                return SensorModel.Name;
            }
        }

        public string MACAddress
        {
            get
            {
                string[] idParts = SensorModel.Id.ToString().Split(new char[] { '-' });
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

        public string GroupName { get; set; }

        public long EndLoggingTime { get; set; }

        private int _batteryLevel;
        public int BatteryLevel { get { return _batteryLevel; } set { Set(() => BatteryLevel, ref _batteryLevel, value); } }

        private string _softwareVersion;
        public string SoftwareVersion { get { return _softwareVersion; } set { Set(() => SoftwareVersion, ref _softwareVersion, value); } }

        private DeviceStatus _deviceStatus;

        public DeviceStatus DeviceStatus
        {
            get { return _deviceStatus; }
            set { Set(() => DeviceStatus, ref _deviceStatus, value); }
        }

        public async Task Connect()
        {
            DeviceStatus = DeviceStatus.Connecting;

            if (SensorModel.Connector == null)
            {
                SensorModel.Connector = new SensorKitMovesenseConnector(SensorModel);
            }

            if (!SensorModel.IsSubscribed)
            {
                await SensorModel.Subscribe();
            }

            Debug.WriteLine("Ble Connected!");

            // Now do the Mds connection
            await new MdsConnectionService().ConnectMdsAsync(MACAddress);

            DeviceStatus = DeviceStatus.Connected;
        }

        public async Task Disconnect()
        {
            DeviceStatus = DeviceStatus.Connecting;

            // Disconnect Mds
            await new MdsConnectionService().DisconnectMds(MACAddress);

            // Disconnect SensorKit
            if (SensorModel.Connector == null)
            {
                SensorModel.Connector = new SensorKitMovesenseConnector(SensorModel);
            }

            if (SensorModel.IsSubscribed)
            {
                await SensorModel.Unsubscribe();
            }

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
