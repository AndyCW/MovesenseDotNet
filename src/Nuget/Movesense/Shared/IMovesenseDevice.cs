using MdsLibrary.Model;
using Plugin.Movesense.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Movesense
{
    /// <summary>
    /// Movesense.NET API for a Movesense Device
    /// </summary>
    public interface IMovesenseDevice
    {
        /// <summary>
        /// Serial number of the connected device
        /// </summary>
        string Serial { get; }

        /// <summary>
        /// Unique ID of the connected device
        /// </summary>
        Guid Uuid { get; }

        /// <summary>
        /// Disconnect the device from MdsLib
        /// </summary>
        /// <returns>null</returns>
        Task<object> DisconnectMdsAsync();

        /// <summary>
        /// Create a new logbook entry resource (increment log Id). Returns the new log Id.
        /// </summary>
        /// <returns>new Log Id</returns>
        Task<CreateLogResult> CreateLogEntryAsync();

        /// <summary>
        /// Delete all the Logbook entries
        /// </summary>
        Task DeleteLogEntriesAsync();

        /// <summary>
        /// Get Accelerometer configuration
        /// </summary>
        Task<AccInfo> GetAccInfoAsync();

        /// <summary>
        /// Get info on the app running on the device
        /// </summary>
        Task<AppInfo> GetAppInfoAsync();

        /// <summary>
        /// Get Battery level, CallAsync returns BatteryResult
        /// </summary>
        Task<BatteryResult> GetBatteryLevelAsync();

        /// <summary>
        /// Gets all current time related info in one go. This enables knowledge of RelativeTime 
        /// and UTC time at the same instance, which is needed in multisensor use cases.
        /// </summary>
        Task<DetailedTime> GetDetailedTimeAsync();

        /// <summary>
        /// Get device info
        /// </summary>
        Task<DeviceInfoResult> GetDeviceInfoAsync();

        /// <summary>
        /// Get Gyrometer configuration
        /// </summary>
        Task<GyroInfo> GetGyroInfoAsync();

        /// <summary>
        /// Get IMU configuration
        /// </summary>
        Task<IMUInfo> GetIMUInfoAsync();

        /// <summary>
        /// Get state of all Leds in the system
        /// </summary>
        Task<LedsResult> GetLedsStateAsync();

        /// <summary>
        /// Get LedState for an LED
        /// </summary>
        /// <param name="ledIndex">Number of the Led</param>
        Task<LedState> GetLedStateAsync(int ledIndex = 0);

        /// <summary>
        /// Get data from a Logbook entry in SBEM format by accessing the suunto://{serial}/Mem/Logbook/ByID/{ID}/Data REST endpoint
        /// </summary>
        /// <param name="logId">Number of the entry to get</param>
        Task<string> GetLogbookDataAsync(int logId);

        /// <summary>
        /// Get data from a Logbook entry as JSON by accessing the suunto://MDS/Logbook/{serial}>/ByID/{ID}/Data REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.  
        /// </summary>
        /// <param name="logId">Number of the entry to get</param>
        Task<string> GetLogbookDataJsonAsync(int logId);

        /// <summary>
        /// Get Descriptors for a Logbook entry
        /// </summary>
        /// <param name="logId">Logbook entry to get</param>
        Task<BaseResult> GetLogbookDescriptorsAsync(int logId);

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://MDS/Logbook/{serial}>/Entries" REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.
        /// </summary>
        Task<LogEntriesMDSResult> GetLogEntriesJsonAsync();

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://{serial}/Mem/Logbook/Entries REST endpoint
        /// </summary>
        Task<LogEntriesResult> GetLogEntriesAsync();

        /// <summary>
        /// Get Logger status, CallAsync returns LogStatusResult object
        /// </summary>
        Task<LogStatusResult> GetLoggerStatusAsync();

        /// <summary>
        /// Get Magnetometer configuration
        /// </summary>
        Task<MagnInfo> GetMagInfoAsync();

        /// <summary>
        /// Gets current time in number of microseconds since epoch 1.1.1970 (UTC).
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        Task<TimeResult> GetTimeAsync();

        /// <summary>
        /// Sets state of an LED
        /// </summary>
        /// <param name="ledIndex">Index of the Led - use 0 for standard Movesense sensor</param>
        /// <param name="ledOn">Set on or off</param>
        /// <param name="ledColor">[optional]value from LedColor enumeration - default is LedColor.Red</param>
        Task SetLedStateAsync(int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red);

        /// <summary>
        /// Set state of the Datalogger
        /// </summary>
        /// <param name="start">Set true to start the datalogger, false to stop</param>
        Task SetLoggerStatusAsync(bool start);

        /// <summary>
        /// Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC).
        /// </summary>
        Task SetTimeAsync();

        /// <summary>
        /// Set configuration for the Datalogger - ONLY sets IMU9 in current implementation
        /// </summary>
        /// <param name="freq">Sampling rate, e.g. 26 for 26Hz</param>
        Task SetupLoggerAsync(int freq = 26);

        /// <summary>
        /// Set configuration for the Datalogger
        /// </summary>
        /// <param name="dataLoggerConfig">Configuration to apply to the DataLogger. Config is an array of structs containing paths to the subscription of data to log.
        /// For example:             
        /// DataLoggerConfig.DataEntry[] entries = { new DataLoggerConfig.DataEntry("/Meas/IMU9/" + freq) };
        /// DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
        /// </param>
        Task SetLoggerConfigAsync(DataLoggerConfig dataLoggerConfig);

        /// <summary>
        /// Subscribe to periodic linear acceleration measurements.
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the AccData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeAccelerometerAsync(Action<AccData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic Gyrometer data
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the GyroData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeGyrometerAsync(Action<GyroData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic 6-axis IMU measurements (Acc + Gyro).
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the IMU6Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeIMU6Async(Action<IMU6Data> notificationCallback, int sampleRate = 26);
        /// <summary>
        /// Subscribe to periodic 9-axis IMU measurements.
        /// </summary>

        /// <param name="notificationCallback">Callback function to receive the IMU9Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeIMU9Async(Action<IMU9Data> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic Magnetometer data measurements
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the MagnData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeMagnetometerAsync(Action<MagnData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to device time notifications
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the time data</param>
        Task<IMdsSubscription> SubscribeTimeAsync(Action<TimeNotificationResult> notificationCallback);
    }
}
