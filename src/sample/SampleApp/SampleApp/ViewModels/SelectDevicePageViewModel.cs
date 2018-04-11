using GalaSoft.MvvmLight;
using SampleApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SampleApp.ViewModels
{
    public class SelectDevicePageViewModel : ViewModelBase
    {
        private IDeviceScanService _deviceScanService;

        public ObservableCollection<MovesenseDeviceViewModel> AllSensors { private set; get; }

        public ICommand UpdateCheckBoxCommand { get; set; }

        public SelectDevicePageViewModel(IDeviceScanService deviceScanService)
        {
            _deviceScanService = deviceScanService;

            AllSensors = new ObservableCollection<MovesenseDeviceViewModel>();

            UpdateCheckBoxCommand = new Xamarin.Forms.Command<Guid>((Id) => UpdateCheckBox(Id));

            Application.Current.Properties.Remove("SelectedSensorId");
        }

        public void Init()
        {
            Application.Current.Properties.Remove("SelectedSensorId");
        }

        private void UpdateCheckBox(Guid id)
        {
            MovesenseDeviceViewModel item = SearchDeviceList(id);
            item.IsSelected = !item.IsSelected;
            if (item.IsSelected)
            {
                if (Application.Current.Properties.ContainsKey("SelectedSensorId"))
                {
                    Application.Current.Properties["SelectedSensorId"] = item.ID.ToString();
                }
                else
                {
                    Application.Current.Properties.Add("SelectedSensorId", item.ID.ToString());
                }
            }
        }

        public void StartScanning()
        {
            _deviceScanService.MovesenseDeviceFound += _deviceScanService_MovesenseDeviceFound;
            _deviceScanService.StartScanning();
        }

        public void StopScanning()
        {
            _deviceScanService.MovesenseDeviceFound -= _deviceScanService_MovesenseDeviceFound;
            _deviceScanService.StopScanning();
        }

        private void _deviceScanService_MovesenseDeviceFound(object sender, MovesenseDeviceFoundArgs e)
        {
            // Have we already seen this one?
            MovesenseDeviceViewModel device = SearchDeviceList(e.Device.Id);

            // If not, put into the group
            if (device == null)
            {
                // No - add to the Unassigned group
                device = new MovesenseDeviceViewModel(e.Device);
                AllSensors.Add(device);
            }

            device.DeviceStatus = DeviceStatus.Connected;
        }

        private MovesenseDeviceViewModel SearchDeviceList(Guid lookfor)
        {
            MovesenseDeviceViewModel device = AllSensors.SingleOrDefault((d) => d.ID == lookfor);

            return device;
        }

    }
}
