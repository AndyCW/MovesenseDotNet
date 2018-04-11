using SampleApp.ViewModels;
using System;
using System.Threading.Tasks;

namespace SampleApp.Services
{
    public interface IDeviceScanService
    {
        event EventHandler<MovesenseDeviceFoundArgs> MovesenseDeviceFound;

        void StartScanning();
        void StopScanning();
    }
}