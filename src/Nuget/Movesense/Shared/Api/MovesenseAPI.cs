using MdsLibrary.Api;
using MdsLibrary.Model;
using Plugin.Movesense.Api;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Plugin.Movesense
{
    /// <summary>
    /// Implements the methods of the IMovesense interface
    /// </summary>
    public partial class MovesenseImplementation
    {
        private static readonly string LOGBOOK_ENTRIES_PATH = "/Mem/Logbook/Entries";
        private static readonly string ACC_INFO_PATH = "/Meas/Acc/Info";
        private static readonly string APP_INFO_PATH = "/Info/App";
        private static readonly string BATTERY_PATH = "/System/Energy/Level";
        private static readonly string LOGBOOK_DATA_PATH = "/Mem/Logbook/byId/{0}/Data";
        private static readonly string MDS_LOGBOOK_PREFIX = "MDS/Logbook/";
        private static readonly string MDS_LOGBOOK_DATA_PATH = "/byId/{0}/Data";
        private static readonly string MDS_LOGBOOK_ENTRIES_PATH = "/Entries";
        private static readonly string LOGBOOK_DATA = "/Mem/Logbook/byId/{0}/Descriptors";
        private static readonly string INFO_PATH = "/Info";
        private static readonly string GYRO_INFO_PATH = "/Meas/Gyro/Info";
        private static readonly string IMU_INFO_PATH = "/Meas/IMU/Info";
        private static readonly string LED_PATH = "/Component/Leds/{0}";
        private static readonly string LEDS_PATH = "/Component/Leds";
        private static readonly string DATALOGGER_STATE_PATH = "/Mem/DataLogger/State/";
        private static readonly string MAG_INFO_PATH = "/Meas/Magn/Info";
        private static readonly string TIME_PATH = "/Time";
        private static readonly string DATALOGGER_CONFIG_PATH = "/Mem/DataLogger/Config/";
        private static readonly string ACCELEROMETER_SUBSCRIPTION_PATH = "/Meas/Acc/{0}";
        private static readonly string GYROMETER_SUBSCRIPTION_PATH = "/Meas/Gyro/{0}";
        private static readonly string MAGNETOMETER_SUBSCRIPTION_PATH = "/Meas/Magn/{0}";
        private static readonly string IMU6_SUBSCRIPTION_PATH = "/Meas/IMU6/{0}";
        private static readonly string IMU9_SUBSCRIPTION_PATH = "/Meas/IMU9/{0}";
        private static readonly string TIME_SUBSCRIPTION_PATH = "/Time";

        private const int DEFAULT_SAMPLE_RATE = 26;

        #region General purpose MdsLib calling methods
        /// <summary>
        /// Function to make Mds API call that does not return a value
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        public async Task ApiCallAsync(MdsConnectionContext connectionContext, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            await new ApiCallAsync(connectionContext, restOp, path, body, prefixPath).CallAsync();
        }

        /// <summary>
        /// Function to make Mds API call that returns a value of type T
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        public async Task<T> ApiCallAsync<T>(MdsConnectionContext connectionContext, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            return await new ApiCallAsync<T>(connectionContext, restOp, path, body, prefixPath).CallAsync();
        }

        /// <summary>
        /// Function to start a subscription to an Mds resource
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="notificationCallback">Callback function that takes parameter of type T, where T is the return type from the subscription notifications</param>
        public async Task<IMdsSubscription> ApiSubscriptionAsync<T>(MdsConnectionContext connectionContext, string path, Action<T> notificationCallback)
        {
            return await new ApiSubscription<T>(connectionContext, path).SubscribeAsync(notificationCallback);
        }

        #endregion

        #region Movesense.NET function methods

        /// <summary>
        /// Create a new logbook entry resource (increment log Id). Returns the new log Id.
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        [Obsolete("Passing argument of connectionContext is deprecated, please use ApiCallAsync(CreateLogEntryAsync) instead.")]
        public async Task<CreateLogResult> CreateLogEntryAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<CreateLogResult>(connectionContext, MdsOp.POST, LOGBOOK_ENTRIES_PATH);
            return await op.CallAsync();
        }
        
        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://{serial}/Mem/Logbook/Entries REST endpoint
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<LogEntriesResult> GetLogEntriesAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<LogEntriesResult>(connectionContext, MdsOp.GET, LOGBOOK_ENTRIES_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://MDS/Logbook/{serial}>/Entries" REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<LogEntriesMDSResult> GetLogEntriesJsonAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<LogEntriesMDSResult>(connectionContext, MdsOp.GET, MDS_LOGBOOK_ENTRIES_PATH, null, MDS_LOGBOOK_PREFIX);
            return await op.CallAsync();
        }

        /// <summary>
        /// Delete all the Logbook entries
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task DeleteLogEntriesAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync(connectionContext, MdsOp.DELETE, LOGBOOK_ENTRIES_PATH);
            await op.CallAsync();
        }


        /// <summary>
        /// Get Accelerometer configuration
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<AccInfo> GetAccInfoAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<AccInfo>(connectionContext, MdsOp.GET, ACC_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Magnetometer configuration
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<MagnInfo> GetMagInfoAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<MagnInfo>(connectionContext, MdsOp.GET, MAG_INFO_PATH);
            return await op.CallAsync();
        }


        /// <summary>
        /// Get Battery level, CallAsync returns BatteryResult
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<BatteryResult> GetBatteryLevelAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<BatteryResult>(connectionContext, MdsOp.GET, BATTERY_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get data from a Logbook entry
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="logId">Number of the entry to get</param>
        public async Task<string> GetLogbookDataAsync(MdsConnectionContext connectionContext, int logId)
        {
            string datapath = String.Format(LOGBOOK_DATA_PATH, logId);
            var op = new ApiCallAsync<string>(connectionContext, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get data from a Logbook entry as Json by accessing the suunto://MDS/Logbook/{serial}>/ByID/{ID}/Data REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.  
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="logId">Number of the entry to get</param>
        public async Task<string> GetLogbookDataJsonAsync(MdsConnectionContext connectionContext, int logId)
        {
            string datapath = String.Format(MDS_LOGBOOK_DATA_PATH, logId);
            var op = new ApiCallAsync<string>(connectionContext, MdsOp.GET, datapath, null, MDS_LOGBOOK_PREFIX);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Descriptors for a Logbook entry
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="logId">Logbook entry to get</param>
        public async Task<BaseResult> GetLogbookDescriptorsAsync(MdsConnectionContext connectionContext, int logId)
        {
            string datapath = String.Format(LOGBOOK_DATA, logId);
            var op = new ApiCallAsync<BaseResult>(connectionContext, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get info on the app running on the device
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<AppInfo> GetAppInfoAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<AppInfo>(connectionContext, MdsOp.GET, APP_INFO_PATH);
            return await op.CallAsync();
        }
        /// <summary>
        /// Get device info
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<DeviceInfoResult> GetDeviceInfoAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<DeviceInfoResult>(connectionContext, MdsOp.GET, INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Gyrometer configuration
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<GyroInfo> GetGyroInfoAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<GyroInfo>(connectionContext, MdsOp.GET, GYRO_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get IMU configuration
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<IMUInfo> GetIMUInfoAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<IMUInfo>(connectionContext, MdsOp.GET, IMU_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get LedState for an LED
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="ledIndex">Number of the Led</param>
        public async Task<LedState> GetLedStateAsync(MdsConnectionContext connectionContext, int ledIndex = 0)
        {
            string datapath = String.Format(LED_PATH, ledIndex);
            var op = new ApiCallAsync<LedState>(connectionContext, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Sets state of an LED
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="ledIndex">Index of the Led - use 0 for standard Movesense sensor</param>
        /// <param name="ledOn">Set on or off</param>
        /// <param name="ledColor">[optional]value from LedColor enumeration - default is LedColor.Red</param>
        public async Task SetLedStateAsync(MdsConnectionContext connectionContext, int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red)
        {
            string datapath = String.Format(LED_PATH, ledIndex);
            string led_On_Body = $"{{ \"LedState\": {{ \"IsOn\": true, \"LedColor\": {(int)ledColor}}} }}";
            string led_Off_Body = @"{ ""LedState"": { ""IsOn"": false, ""LedColor"": 0} }";
            var op = new ApiCallAsync<LedState>(connectionContext, MdsOp.PUT, datapath, ledOn ? led_On_Body : led_Off_Body);
            await op.CallAsync();
        }

        /// <summary>
        /// Get state of all Leds in the system
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<LedsResult> GetLedsStateAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<LedsResult>(connectionContext, MdsOp.GET, LEDS_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Logger status, CallAsync returns LogStatusResult object
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<LogStatusResult> GetLoggerStatusAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<LogStatusResult>(connectionContext, MdsOp.GET, DATALOGGER_STATE_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Set state of the Datalogger
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="start">Set true to start the datalogger, false to stop</param>
        public async Task SetLoggerStatusAsync(MdsConnectionContext connectionContext, bool start)
        {
            string LOG_ON = "{\"newState\":3}";
            string LOG_OFF = "{\"newState\":2}";
            var op = new ApiCallAsync(connectionContext, MdsOp.PUT, DATALOGGER_STATE_PATH, start ? LOG_ON : LOG_OFF);
            await op.CallAsync();
        }

        /// <summary>
        /// Set configuration for the Datalogger - ONLY sets IMU9
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="freq">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task SetupLoggerAsync(MdsConnectionContext connectionContext, int freq = 26)
        {
            DataLoggerConfig.DataEntry[] entries = {
                new DataLoggerConfig.DataEntry("/Meas/IMU9/" + freq)
            };

            DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
            string jsonConfig = Newtonsoft.Json.JsonConvert.SerializeObject(config);

            var op = new ApiCallAsync(connectionContext, MdsOp.PUT, DATALOGGER_CONFIG_PATH, jsonConfig);
            await op.CallAsync();
        }
        /// <summary>
        /// Set configuration for the Datalogger
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="dataLoggerConfig">Configuration to apply to the DataLogger. Config is an array of structs containing paths to the subscription of data to log.
        /// For example:             
        /// DataLoggerConfig.DataEntry[] entries = { new DataLoggerConfig.DataEntry("/Meas/IMU9/52") };
        /// DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
        /// </param>
        public async Task SetLoggerConfigAsync(MdsConnectionContext connectionContext, DataLoggerConfig dataLoggerConfig)
        {
            string jsonConfig = Newtonsoft.Json.JsonConvert.SerializeObject(dataLoggerConfig);

            var op = new ApiCallAsync(connectionContext, MdsOp.PUT, DATALOGGER_CONFIG_PATH, jsonConfig);
            await op.CallAsync();
        }

        /// <summary>
        /// Gets current time in number of microseconds since epoch 1.1.1970 (UTC).
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task<TimeResult> GetTimeAsync(MdsConnectionContext connectionContext)
        {
            var op = new ApiCallAsync<TimeResult>(connectionContext, MdsOp.GET, TIME_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC).
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        public async Task SetTimeAsync(MdsConnectionContext connectionContext)
        {
            long timems = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            string time = $"{{\"value\":{timems * 1000}}}";
            Debug.WriteLine($"INFO SetTime TIME {time}");
            var op = new ApiCallAsync<TimeResult>(connectionContext, MdsOp.POST, TIME_PATH, time);
            await op.CallAsync();
        }

        /// <summary>
        /// Subscribe to periodic linear acceleration measurements.
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="notificationCallback">Callback function to receive the AccData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeAccelerometerAsync(MdsConnectionContext connectionContext, Action<AccData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(ACCELEROMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<AccData>(connectionContext, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic Gyrometer data
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="notificationCallback">Callback function to receive the GyroData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeGyrometerAsync(MdsConnectionContext connectionContext, Action<GyroData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(GYROMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<GyroData>(connectionContext, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic 6-axis IMU measurements (Acc + Gyro).
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="notificationCallback">Callback function to receive the IMU6Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeIMU6Async(MdsConnectionContext connectionContext, Action<IMU6Data> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(IMU6_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<IMU6Data>(connectionContext, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic 9-axis IMU measurements.
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="notificationCallback">Callback function to receive the IMU9Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeIMU9Async(MdsConnectionContext connectionContext, Action<IMU9Data> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(IMU9_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<IMU9Data>(connectionContext, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic Magnetometer data measurements
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="notificationCallback">Callback function to receive the MagnData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeMagnetometerAsync(MdsConnectionContext connectionContext, Action<MagnData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(MAGNETOMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<MagnData>(connectionContext, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to device time notifications
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext of the device</param>
        /// <param name="notificationCallback">Callback function to receive the time data</param>
        public async Task<IMdsSubscription> SubscribeTimeAsync(MdsConnectionContext connectionContext, Action<TimeNotificationResult> notificationCallback)
        {
            var op = new ApiSubscription<TimeNotificationResult>(connectionContext, TIME_SUBSCRIPTION_PATH);
            return await op.SubscribeAsync(notificationCallback);
        }

        #endregion

        #region Deprecated methods

        /// <summary>
        /// Function to make Mds API call that does not return a value
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use ApiCallAsync(MdsConnectionContext, ...) instead.")]
        public async Task ApiCallAsync(string deviceName, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            await new ApiCallAsync(deviceName, restOp, path, body, prefixPath).CallAsync();
        }

        /// <summary>
        /// Function to make Mds API call that returns a value of type T
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use ApiCallAsync<T>(MdsConnectionContext, ...) instead.")]
        public async Task<T> ApiCallAsync<T>(string deviceName, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            return await new ApiCallAsync<T>(deviceName, restOp, path, body, prefixPath).CallAsync();
        }

        /// <summary>
        /// Function to start a subscription to an Mds resource
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="notificationCallback">Callback function that takes parameter of type T, where T is the return type from the subscription notifications</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use ApiCallAsync(CreateLogEntryAsync) instead.")]
        public async Task<IMdsSubscription> ApiSubscriptionAsync<T>(string deviceName, string path, Action<T> notificationCallback)
        {
            return await new ApiSubscription<T>(deviceName, path).SubscribeAsync(notificationCallback);
        }


        /// <summary>
        /// Create a new logbook entry resource (increment log Id). Returns the new log Id.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use ApiCallAsync(CreateLogEntryAsync) instead.")]
        public async Task<CreateLogResult> CreateLogEntryAsync(string deviceName)
        {
            var op = new ApiCallAsync<CreateLogResult>(deviceName, MdsOp.POST, LOGBOOK_ENTRIES_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://{serial}/Mem/Logbook/Entries REST endpoint
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLogEntriesAsync(MdsConnectionContext) instead.")]
        public async Task<LogEntriesResult> GetLogEntriesAsync(string deviceName)
        {
            var op = new ApiCallAsync<LogEntriesResult>(deviceName, MdsOp.GET, LOGBOOK_ENTRIES_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://MDS/Logbook/{serial}>/Entries" REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLogEntriesJsonAsync(MdsConnectionContext) instead.")]
        public async Task<LogEntriesMDSResult> GetLogEntriesJsonAsync(string deviceName)
        {
            var op = new ApiCallAsync<LogEntriesMDSResult>(deviceName, MdsOp.GET, MDS_LOGBOOK_ENTRIES_PATH, null, MDS_LOGBOOK_PREFIX);
            return await op.CallAsync();
        }

        /// <summary>
        /// Delete all the Logbook entries
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use DeleteLogEntriesAsync(MdsConnectionContext) instead.")]
        public async Task DeleteLogEntriesAsync(string deviceName)
        {
            var op = new ApiCallAsync(deviceName, MdsOp.DELETE, LOGBOOK_ENTRIES_PATH);
            await op.CallAsync();
        }


        /// <summary>
        /// Get Accelerometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetAccInfoAsync(MdsConnectionContext) instead.")]
        public async Task<AccInfo> GetAccInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<AccInfo>(deviceName, MdsOp.GET, ACC_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Magnetometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetMagInfoAsync(MdsConnectionContext) instead.")]
        public async Task<MagnInfo> GetMagInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<MagnInfo>(deviceName, MdsOp.GET, MAG_INFO_PATH);
            return await op.CallAsync();
        }


        /// <summary>
        /// Get Battery level, CallAsync returns BatteryResult
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetBatteryLevelAsync(MdsConnectionContext) instead.")]
        public async Task<BatteryResult> GetBatteryLevelAsync(string deviceName)
        {
            var op = new ApiCallAsync<BatteryResult>(deviceName, MdsOp.GET, BATTERY_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get data from a Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Number of the entry to get</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLogbookDataAsync(MdsConnectionContext, int) instead.")]
        public async Task<string> GetLogbookDataAsync(string deviceName, int logId)
        {
            string datapath = String.Format(LOGBOOK_DATA_PATH, logId);
            var op = new ApiCallAsync<string>(deviceName, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get data from a Logbook entry as Json by accessing the suunto://MDS/Logbook/{serial}>/ByID/{ID}/Data REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.  
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Number of the entry to get</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLogbookDataJsonAsync(MdsConnectionContext, int) instead.")]
        public async Task<string> GetLogbookDataJsonAsync(string deviceName, int logId)
        {
            string datapath = String.Format(MDS_LOGBOOK_DATA_PATH, logId);
            var op = new ApiCallAsync<string>(deviceName, MdsOp.GET, datapath, null, MDS_LOGBOOK_PREFIX);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Descriptors for a Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Logbook entry to get</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLogbookDataJsonAsync(MdsConnectionContext, int) instead.")]
        public async Task<BaseResult> GetLogbookDescriptorsAsync(string deviceName, int logId)
        {
            string datapath = String.Format(LOGBOOK_DATA, logId);
            var op = new ApiCallAsync<BaseResult>(deviceName, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get info on the app running on the device
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetAppInfoAsync(MdsConnectionContext) instead.")]
        public async Task<AppInfo> GetAppInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<AppInfo>(deviceName, MdsOp.GET, APP_INFO_PATH);
            return await op.CallAsync();
        }
        /// <summary>
        /// Get device info
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetDeviceInfoAsync(MdsConnectionContext) instead.")]
        public async Task<DeviceInfoResult> GetDeviceInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<DeviceInfoResult>(deviceName, MdsOp.GET, INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Gyrometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetGyroInfoAsync(MdsConnectionContext) instead.")]
        public async Task<GyroInfo> GetGyroInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<GyroInfo>(deviceName, MdsOp.GET, GYRO_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get IMU configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetIMUInfoAsync(MdsConnectionContext) instead.")]
        public async Task<IMUInfo> GetIMUInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<IMUInfo>(deviceName, MdsOp.GET, IMU_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get LedState for an LED
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="ledIndex">Number of the Led</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLedStateAsync(MdsConnectionContext, int) instead.")]
        public async Task<LedState> GetLedStateAsync(string deviceName, int ledIndex = 0)
        {
            string datapath = String.Format(LED_PATH, ledIndex);
            var op = new ApiCallAsync<LedState>(deviceName, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Sets state of an LED
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="ledIndex">Index of the Led - use 0 for standard Movesense sensor</param>
        /// <param name="ledOn">Set on or off</param>
        /// <param name="ledColor">[optional]value from LedColor enumeration - default is LedColor.Red</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SetLedStateAsync(MdsConnectionContext...) instead.")]
        public async Task SetLedStateAsync(string deviceName, int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red)
        {
            string datapath = String.Format(LED_PATH, ledIndex);
            string led_On_Body = $"{{ \"LedState\": {{ \"IsOn\": true, \"LedColor\": {(int)ledColor}}} }}";
            string led_Off_Body = @"{ ""LedState"": { ""IsOn"": false, ""LedColor"": 0} }";
            var op = new ApiCallAsync<LedState>(deviceName, MdsOp.PUT, datapath, ledOn ? led_On_Body : led_Off_Body);
            await op.CallAsync();
        }

        /// <summary>
        /// Get state of all Leds in the system
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLedsStateAsync(MdsConnectionContext) instead.")]
        public async Task<LedsResult> GetLedsStateAsync(string deviceName)
        {
            var op = new ApiCallAsync<LedsResult>(deviceName, MdsOp.GET, LEDS_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Logger status, CallAsync returns LogStatusResult object
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLoggerStatusAsync(MdsConnectionContext) instead.")]
        public async Task<LogStatusResult> GetLoggerStatusAsync(string deviceName)
        {
            var op = new ApiCallAsync<LogStatusResult>(deviceName, MdsOp.GET, DATALOGGER_STATE_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Set state of the Datalogger
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="start">Set true to start the datalogger, false to stop</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SetLoggerStatusAsync(MdsConnectionContext, bool) instead.")]
        public async Task SetLoggerStatusAsync(string deviceName, bool start)
        {
            string LOG_ON = "{\"newState\":3}";
            string LOG_OFF = "{\"newState\":2}";
            var op = new ApiCallAsync(deviceName, MdsOp.PUT, DATALOGGER_STATE_PATH, start ? LOG_ON : LOG_OFF);
            await op.CallAsync();
        }

        /// <summary>
        /// Set configuration for the Datalogger - ONLY sets IMU9
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="freq">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SetupLoggerAsync(MdsConnectionContext, int) instead.")]
        public async Task SetupLoggerAsync(string deviceName, int freq = 26)
        {
            DataLoggerConfig.DataEntry[] entries = {
                new DataLoggerConfig.DataEntry("/Meas/IMU9/" + freq)
            };

            DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
            string jsonConfig = Newtonsoft.Json.JsonConvert.SerializeObject(config);

            var op = new ApiCallAsync(deviceName, MdsOp.PUT, DATALOGGER_CONFIG_PATH, jsonConfig);
            await op.CallAsync();
        }
        /// <summary>
        /// Set configuration for the Datalogger
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="dataLoggerConfig">Configuration to apply to the DataLogger. Config is an array of structs containing paths to the subscription of data to log.
        /// For example:             
        /// DataLoggerConfig.DataEntry[] entries = { new DataLoggerConfig.DataEntry("/Meas/IMU9/52") };
        /// DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
        /// </param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SetLoggerConfigAsync(MdsConnectionContext, DataLoggerConfig) instead.")]
        public async Task SetLoggerConfigAsync(string deviceName, DataLoggerConfig dataLoggerConfig)
        {
            string jsonConfig = Newtonsoft.Json.JsonConvert.SerializeObject(dataLoggerConfig);

            var op = new ApiCallAsync(deviceName, MdsOp.PUT, DATALOGGER_CONFIG_PATH, jsonConfig);
            await op.CallAsync();
        }

        /// <summary>
        /// Gets current time in number of microseconds since epoch 1.1.1970 (UTC).
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetTimeAsync(MdsConnectionContext) instead.")]
        public async Task<TimeResult> GetTimeAsync(string deviceName)
        {
            var op = new ApiCallAsync<TimeResult>(deviceName, MdsOp.GET, TIME_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC).
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SetTimeAsync(MdsConnectionContext) instead.")]
        public async Task SetTimeAsync(string deviceName)
        {
            long timems = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            string time = $"{{\"value\":{timems * 1000}}}";
            Debug.WriteLine($"INFO SetTime TIME {time}");
            var op = new ApiCallAsync<TimeResult>(deviceName, MdsOp.POST, TIME_PATH, time);
            await op.CallAsync();
        }

        /// <summary>
        /// Subscribe to periodic linear acceleration measurements.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the AccData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeAccelerometerAsync(MdsConnectionContext...) instead.")]
        public async Task<IMdsSubscription> SubscribeAccelerometerAsync(string deviceName, Action<AccData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(ACCELEROMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<AccData>(deviceName, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic Gyrometer data
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the GyroData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeGyrometerAsync(MdsConnectionContext...) instead.")]
        public async Task<IMdsSubscription> SubscribeGyrometerAsync(string deviceName, Action<GyroData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(GYROMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<GyroData>(deviceName, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic 6-axis IMU measurements (Acc + Gyro).
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the IMU6Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeIMU6Async(MdsConnectionContext...) instead.")]
        public async Task<IMdsSubscription> SubscribeIMU6Async(string deviceName, Action<IMU6Data> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(IMU6_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<IMU6Data>(deviceName, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic 9-axis IMU measurements.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the IMU9Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeIMU9Async(MdsConnectionContext...) instead.")]
        public async Task<IMdsSubscription> SubscribeIMU9Async(string deviceName, Action<IMU9Data> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(IMU9_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<IMU9Data>(deviceName, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic Magnetometer data measurements
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the MagnData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeMagnetometerAsync(MdsConnectionContext...) instead.")]
        public async Task<IMdsSubscription> SubscribeMagnetometerAsync(string deviceName, Action<MagnData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(MAGNETOMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<MagnData>(deviceName, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to device time notifications
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the time data</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeTimeAsync(MdsConnectionContext...) instead.")]
        public async Task<IMdsSubscription> SubscribeTimeAsync(string deviceName, Action<TimeNotificationResult> notificationCallback)
        {
            var op = new ApiSubscription<TimeNotificationResult>(deviceName, TIME_SUBSCRIPTION_PATH);
            return await op.SubscribeAsync(notificationCallback);
        }

        #endregion

    }
}
