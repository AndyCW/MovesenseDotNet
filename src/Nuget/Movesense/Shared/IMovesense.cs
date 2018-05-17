using MdsLibrary.Model;
using Plugin.Movesense.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Movesense
{
    public interface IMovesense
    {
        object MdsInstance { get; }
        object Activity { set; }
        string SCHEME_PREFIX { get; }

        Task<CreateLogResult> CreateLogEntryAsync(string deviceName);
        Task DeleteLogEntriesAsync(string deviceName);
        Task<AccInfo> GetAccInfoAsync(string deviceName);
        Task<BatteryResult> GetBatteryLevelAsync(string deviceName);
        Task<DeviceInfoResult> GetDeviceInfoAsync(string deviceName);
        Task<GyroInfo> GetGyroInfoAsync(string deviceName);
        Task<IMUInfo> GetIMUInfoAsync(string deviceName);
        Task<LedsResult> GetLedsStateAsync(string deviceName);
        Task<LedState> GetLedStateAsync(string deviceName, int ledIndex = 0);
        Task<BaseResult> GetLogbookDataAsync(string deviceName, int logId);
        Task<BaseResult> GetLogbookDescriptorsAsync(string deviceName, int logId);
        Task<LogEntriesResult> GetLogEntriesAsync(string deviceName);
        Task<LogStatusResult> GetLoggerStatusAsync(string deviceName);
        Task<MagnInfo> GetMagInfoAsync(string deviceName);
        Task<TimeResult> GetTimeAsync(string deviceName);
        Task SetLedStateAsync(string deviceName, int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red);
        Task SetLoggerStatusAsync(string deviceName, bool start);
        Task SetTimeAsync(string deviceName);
        Task SetupLoggerAsync(string deviceName, int freq = 26);
        Task SubscribeAccelerometerAsync(string deviceName, Action<AccData> notificationCallback, int sampleRate = 26);
        Task SubscribeGyrometerAsync(string deviceName, Action<GyroData> notificationCallback, int sampleRate = 26);
        Task SubscribeIMU6Async(string deviceName, Action<IMU6Data> notificationCallback, int sampleRate = 26);
        Task SubscribeIMU9Async(string deviceName, Action<IMU9Data> notificationCallback, int sampleRate = 26);
        Task SubscribeMagnetometerAsync(string deviceName, Action<MagnData> notificationCallback, int sampleRate = 26);
    }
}
