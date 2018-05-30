using MdsLibrary.Model;
using Plugin.Movesense.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Movesense
{
    /// <summary>
    /// Movesense Plugin API
    /// </summary>
    public interface IMovesense
    {
        /// <summary>
        /// Gets the native MdsLib object
        /// </summary>
        object MdsInstance { get; }

        /// <summary>
        /// On Android, this must be set to the current Activity before first access of the library.
        /// </summary>
        object Activity { set; }

        /// <summary>
        /// Root of all paths to Movesense resources.
        /// </summary>
        string SCHEME_PREFIX { get; }

        /// <summary>
        /// Connect a device to MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>null</returns>
        Task<object> ConnectMdsAsync(Guid Uuid);

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>null</returns>
        Task<object> DisconnectMdsAsync(Guid Uuid);

        /// <summary>
        /// Function to make Mds API call that does not return a value
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        Task ApiCallAsync(string deviceName, MdsOp restOp, string path, string body = null);

        /// <summary>
        /// Function to make Mds API call that returns a value of type T
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        Task<T> ApiCallAsync<T>(string deviceName, MdsOp restOp, string path, string body = null);

        /// <summary>
        /// Function to start a subscription to an Mds resource
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="frequency">Sample rate, e.g. 52 for 52Hz</param>
        /// <param name="notificationCallback">Callback function that takes parameter of type T, where T is the return type from the subscription notifications</param>
        Task<IMdsSubscription> ApiSubscriptionAsync<T>(string deviceName, string path, int frequency, Action<T> notificationCallback);

        /// <summary>
        /// Create a new logbook entry resource (increment log Id). Returns the new log Id.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <returns>new Log Id</returns>
        Task<CreateLogResult> CreateLogEntryAsync(string deviceName);

        /// <summary>
        /// Delete all the Logbook entries
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task DeleteLogEntriesAsync(string deviceName);

        /// <summary>
        /// Get Accelerometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<AccInfo> GetAccInfoAsync(string deviceName);

        /// <summary>
        /// Get Battery level, CallAsync returns BatteryResult
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<BatteryResult> GetBatteryLevelAsync(string deviceName);

        /// <summary>
        /// Get info on the app running on the device
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<AppInfo> GetAppInfoAsync(string deviceName);

        /// <summary>
        /// Get device info
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<DeviceInfoResult> GetDeviceInfoAsync(string deviceName);

        /// <summary>
        /// Get Gyrometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<GyroInfo> GetGyroInfoAsync(string deviceName);

        /// <summary>
        /// Get IMU configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<IMUInfo> GetIMUInfoAsync(string deviceName);

        /// <summary>
        /// Get state of all Leds in the system
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<LedsResult> GetLedsStateAsync(string deviceName);

        /// <summary>
        /// Get LedState for an LED
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="ledIndex">Number of the Led</param>
        Task<LedState> GetLedStateAsync(string deviceName, int ledIndex = 0);

        /// <summary>
        /// Get data from a Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Number of the entry to get</param>
        Task<BaseResult> GetLogbookDataAsync(string deviceName, int logId);

        /// <summary>
        /// Get Descriptors for a Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Logbook entry to get</param>
        Task<BaseResult> GetLogbookDescriptorsAsync(string deviceName, int logId);

        /// <summary>
        /// Get details of Logbook entries
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<LogEntriesResult> GetLogEntriesAsync(string deviceName);

        /// <summary>
        /// Get Logger status, CallAsync returns LogStatusResult object
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<LogStatusResult> GetLoggerStatusAsync(string deviceName);

        /// <summary>
        /// Get Magnetometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<MagnInfo> GetMagInfoAsync(string deviceName);

        /// <summary>
        /// Gets current time in number of microseconds since epoch 1.1.1970 (UTC).
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task<TimeResult> GetTimeAsync(string deviceName);

        /// <summary>
        /// Sets state of an LED
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="ledIndex">Index of the Led - use 0 for standard Movesense sensor</param>
        /// <param name="ledOn">Set on or off</param>
        /// <param name="ledColor">[optional]value from LedColor enumeration - default is LedColor.Red</param>
        Task SetLedStateAsync(string deviceName, int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red);

        /// <summary>
        /// Set state of the Datalogger
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="start">Set true to start the datalogger, false to stop</param>
        Task SetLoggerStatusAsync(string deviceName, bool start);

        /// <summary>
        /// Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC).
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        Task SetTimeAsync(string deviceName);

        /// <summary>
        /// Set configuration for the Datalogger - ONLY sets IMU9
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="freq">Sampling rate, e.g. 26 for 26Hz</param>
        Task SetupLoggerAsync(string deviceName, int freq = 26);

        /// <summary>
        /// Subscribe to periodic linear acceleration measurements.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the AccData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeAccelerometerAsync(string deviceName, Action<AccData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic Gyrometer data
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the GyroData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeGyrometerAsync(string deviceName, Action<GyroData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic 6-axis IMU measurements (Acc + Gyro).
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the IMU6Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeIMU6Async(string deviceName, Action<IMU6Data> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic 9-axis IMU measurements.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the IMU9Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeIMU9Async(string deviceName, Action<IMU9Data> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic Magnetometer data measurements
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the MagnData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeMagnetometerAsync(string deviceName, Action<MagnData> notificationCallback, int sampleRate = 26);
    }
}
