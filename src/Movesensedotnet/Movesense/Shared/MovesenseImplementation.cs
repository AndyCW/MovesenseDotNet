using MdsLibrary;
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

        #region Properties

        /// <summary>
        /// Get the MdsConnectionListener instance.
        /// </summary>
        public MdsConnectionListener ConnectionListener => MdsConnectionListener.Current;

        #endregion

        #region General purpose MdsLib calling methods
        /// <summary>
        /// Function to make Mds API call that does not return a value
        /// </summary>
        /// <param name="movesenseDevice">IMovesenseDevice for the device</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        public async Task ApiCallAsync(IMovesenseDevice movesenseDevice, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            await new ApiCallAsync(movesenseDevice, restOp, path, body, prefixPath).CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Function to make Mds API call that returns a value of type T
        /// </summary>
        /// <param name="movesenseDevice">IMovesenseDevice for the device</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        public async Task<T> ApiCallAsync<T>(IMovesenseDevice movesenseDevice, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            return await new ApiCallAsync<T>(movesenseDevice, restOp, path, body, prefixPath).CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Function to start a subscription to an Mds resource
        /// </summary>
        /// <param name="movesenseDevice">IMovesenseDevice for the device</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="notificationCallback">Callback function that takes parameter of type T, where T is the return type from the subscription notifications</param>
        public async Task<IMdsSubscription> ApiSubscriptionAsync<T>(IMovesenseDevice movesenseDevice, string path, Action<T> notificationCallback)
        {
            return await new ApiSubscription<T>(movesenseDevice, path).SubscribeAsync(notificationCallback).ConfigureAwait(false);
        }

        #endregion

        #region Deprecated methods

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

        /// <summary>
        /// Function to make Mds API call that does not return a value
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use ApiCallAsync(MdsMovesenseDevice, ...) instead.")]
        public async Task ApiCallAsync(string deviceName, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            await new ApiCallAsync(deviceName, restOp, path, body, prefixPath).CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Function to make Mds API call that returns a value of type T
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use ApiCallAsync<T>(MdsMovesenseDevice, ...) instead.")]
        public async Task<T> ApiCallAsync<T>(string deviceName, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            return await new ApiCallAsync<T>(deviceName, restOp, path, body, prefixPath).CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Function to start a subscription to an Mds resource
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="notificationCallback">Callback function that takes parameter of type T, where T is the return type from the subscription notifications</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use ApiSubscriptionAsync<T>(MdsMovesenseDevice, ...) instead.")]
        public async Task<IMdsSubscription> ApiSubscriptionAsync<T>(string deviceName, string path, Action<T> notificationCallback)
        {
            return await new ApiSubscription<T>(deviceName, path).SubscribeAsync(notificationCallback).ConfigureAwait(false);
        }


        /// <summary>
        /// Create a new logbook entry resource (increment log Id). Returns the new log Id.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.CreateLogEntryAsync instead.")]
        public async Task<CreateLogResult> CreateLogEntryAsync(string deviceName)
        {
            var op = new ApiCallAsync<CreateLogResult>(deviceName, MdsOp.POST, LOGBOOK_ENTRIES_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://{serial}/Mem/Logbook/Entries REST endpoint
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetLogEntriesAsync instead.")]
        public async Task<LogEntriesResult> GetLogEntriesAsync(string deviceName)
        {
            var op = new ApiCallAsync<LogEntriesResult>(deviceName, MdsOp.GET, LOGBOOK_ENTRIES_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://MDS/Logbook/{serial}>/Entries" REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetLogEntriesJsonAsync instead.")]
        public async Task<LogEntriesMDSResult> GetLogEntriesJsonAsync(string deviceName)
        {
            var op = new ApiCallAsync<LogEntriesMDSResult>(deviceName, MdsOp.GET, MDS_LOGBOOK_ENTRIES_PATH, null, MDS_LOGBOOK_PREFIX);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Delete all the Logbook entries
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.DeleteLogEntriesAsync instead.")]
        public async Task DeleteLogEntriesAsync(string deviceName)
        {
            var op = new ApiCallAsync(deviceName, MdsOp.DELETE, LOGBOOK_ENTRIES_PATH);
            await op.CallAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// Get Accelerometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetAccInfoAsync instead.")]
        public async Task<AccInfo> GetAccInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<AccInfo>(deviceName, MdsOp.GET, ACC_INFO_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get Magnetometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetMagInfoAsync instead.")]
        public async Task<MagnInfo> GetMagInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<MagnInfo>(deviceName, MdsOp.GET, MAG_INFO_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// Get Battery level, CallAsync returns BatteryResult
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetBatteryLevelAsync instead.")]
        public async Task<BatteryResult> GetBatteryLevelAsync(string deviceName)
        {
            var op = new ApiCallAsync<BatteryResult>(deviceName, MdsOp.GET, BATTERY_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get data from a Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Number of the entry to get</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetLogbookDataAsync(MdsConnectionContext, int) instead.")]
        public async Task<string> GetLogbookDataAsync(string deviceName, int logId)
        {
            string datapath = String.Format(LOGBOOK_DATA_PATH, logId);
            var op = new ApiCallAsync<string>(deviceName, MdsOp.GET, datapath);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get data from a Logbook entry as Json by accessing the suunto://MDS/Logbook/{serial}>/ByID/{ID}/Data REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.  
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Number of the entry to get</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetLogbookDataJsonAsync(MdsConnectionContext, int) instead.")]
        public async Task<string> GetLogbookDataJsonAsync(string deviceName, int logId)
        {
            string datapath = String.Format(MDS_LOGBOOK_DATA_PATH, logId);
            var op = new ApiCallAsync<string>(deviceName, MdsOp.GET, datapath, null, MDS_LOGBOOK_PREFIX);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get Descriptors for a Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Logbook entry to get</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetLogbookDataJsonAsync(MdsConnectionContext, int) instead.")]
        public async Task<BaseResult> GetLogbookDescriptorsAsync(string deviceName, int logId)
        {
            string datapath = String.Format(LOGBOOK_DATA, logId);
            var op = new ApiCallAsync<BaseResult>(deviceName, MdsOp.GET, datapath);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get info on the app running on the device
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetAppInfoAsync instead.")]
        public async Task<AppInfo> GetAppInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<AppInfo>(deviceName, MdsOp.GET, APP_INFO_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// Get device info
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetDeviceInfoAsync instead.")]
        public async Task<DeviceInfoResult> GetDeviceInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<DeviceInfoResult>(deviceName, MdsOp.GET, INFO_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get Gyrometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetGyroInfoAsync instead.")]
        public async Task<GyroInfo> GetGyroInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<GyroInfo>(deviceName, MdsOp.GET, GYRO_INFO_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get IMU configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetIMUInfoAsync instead.")]
        public async Task<IMUInfo> GetIMUInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<IMUInfo>(deviceName, MdsOp.GET, IMU_INFO_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get LedState for an LED
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="ledIndex">Number of the Led</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetLedStateAsync(MdsConnectionContext, int) instead.")]
        public async Task<LedState> GetLedStateAsync(string deviceName, int ledIndex = 0)
        {
            string datapath = String.Format(LED_PATH, ledIndex);
            var op = new ApiCallAsync<LedState>(deviceName, MdsOp.GET, datapath);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Sets state of an LED
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="ledIndex">Index of the Led - use 0 for standard Movesense sensor</param>
        /// <param name="ledOn">Set on or off</param>
        /// <param name="ledColor">[optional]value from LedColor enumeration - default is LedColor.Red</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SetLedStateAsync(MdsConnectionContext...) instead.")]
        public async Task SetLedStateAsync(string deviceName, int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red)
        {
            string datapath = String.Format(LED_PATH, ledIndex);
            string led_On_Body = $"{{ \"LedState\": {{ \"IsOn\": true, \"LedColor\": {(int)ledColor}}} }}";
            string led_Off_Body = @"{ ""LedState"": { ""IsOn"": false, ""LedColor"": 0} }";
            var op = new ApiCallAsync<LedState>(deviceName, MdsOp.PUT, datapath, ledOn ? led_On_Body : led_Off_Body);
            await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get state of all Leds in the system
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetLedsStateAsync instead.")]
        public async Task<LedsResult> GetLedsStateAsync(string deviceName)
        {
            var op = new ApiCallAsync<LedsResult>(deviceName, MdsOp.GET, LEDS_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get Logger status, CallAsync returns LogStatusResult object
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetLoggerStatusAsync instead.")]
        public async Task<LogStatusResult> GetLoggerStatusAsync(string deviceName)
        {
            var op = new ApiCallAsync<LogStatusResult>(deviceName, MdsOp.GET, DATALOGGER_STATE_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Set state of the Datalogger
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="start">Set true to start the datalogger, false to stop</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SetLoggerStatusAsync(...) instead.")]
        public async Task SetLoggerStatusAsync(string deviceName, bool start)
        {
            string LOG_ON = "{\"newState\":3}";
            string LOG_OFF = "{\"newState\":2}";
            var op = new ApiCallAsync(deviceName, MdsOp.PUT, DATALOGGER_STATE_PATH, start ? LOG_ON : LOG_OFF);
            await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Set configuration for the Datalogger - ONLY sets IMU9
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="freq">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SetupLoggerAsync(int) instead.")]
        public async Task SetupLoggerAsync(string deviceName, int freq = 26)
        {
            DataLoggerConfig.DataEntry[] entries = {
                new DataLoggerConfig.DataEntry("/Meas/IMU9/" + freq)
            };

            DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
            string jsonConfig = Newtonsoft.Json.JsonConvert.SerializeObject(config);

            var op = new ApiCallAsync(deviceName, MdsOp.PUT, DATALOGGER_CONFIG_PATH, jsonConfig);
            await op.CallAsync().ConfigureAwait(false);
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
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SetLoggerConfigAsync(DataLoggerConfig) instead.")]
        public async Task SetLoggerConfigAsync(string deviceName, DataLoggerConfig dataLoggerConfig)
        {
            string jsonConfig = Newtonsoft.Json.JsonConvert.SerializeObject(dataLoggerConfig);

            var op = new ApiCallAsync(deviceName, MdsOp.PUT, DATALOGGER_CONFIG_PATH, jsonConfig);
            await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets current time in number of microseconds since epoch 1.1.1970 (UTC).
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.GetTimeAsync instead.")]
        public async Task<TimeResult> GetTimeAsync(string deviceName)
        {
            var op = new ApiCallAsync<TimeResult>(deviceName, MdsOp.GET, TIME_PATH);
            return await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC).
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SetTimeAsync instead.")]
        public async Task SetTimeAsync(string deviceName)
        {
            long timems = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            string time = $"{{\"value\":{timems * 1000}}}";
            Debug.WriteLine($"INFO SetTime TIME {time}");
            var op = new ApiCallAsync<TimeResult>(deviceName, MdsOp.PUT, TIME_PATH, time);
            await op.CallAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to periodic linear acceleration measurements.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the AccData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SubscribeAccelerometerAsync(...) instead.")]
        public async Task<IMdsSubscription> SubscribeAccelerometerAsync(string deviceName, Action<AccData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(ACCELEROMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<AccData>(deviceName, datapath);
            return await op.SubscribeAsync(notificationCallback).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to periodic Gyrometer data
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the GyroData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SubscribeGyrometerAsync(...) instead.")]
        public async Task<IMdsSubscription> SubscribeGyrometerAsync(string deviceName, Action<GyroData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(GYROMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<GyroData>(deviceName, datapath);
            return await op.SubscribeAsync(notificationCallback).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to periodic 6-axis IMU measurements (Acc + Gyro).
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the IMU6Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SubscribeIMU6Async(...) instead.")]
        public async Task<IMdsSubscription> SubscribeIMU6Async(string deviceName, Action<IMU6Data> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(IMU6_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<IMU6Data>(deviceName, datapath);
            return await op.SubscribeAsync(notificationCallback).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to periodic 9-axis IMU measurements.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the IMU9Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SubscribeIMU9Async(...) instead.")]
        public async Task<IMdsSubscription> SubscribeIMU9Async(string deviceName, Action<IMU9Data> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(IMU9_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<IMU9Data>(deviceName, datapath);
            return await op.SubscribeAsync(notificationCallback).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to periodic Magnetometer data measurements
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the MagnData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SubscribeMagnetometerAsync(...) instead.")]
        public async Task<IMdsSubscription> SubscribeMagnetometerAsync(string deviceName, Action<MagnData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(MAGNETOMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<MagnData>(deviceName, datapath);
            return await op.SubscribeAsync(notificationCallback).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to device time notifications
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the time data</param>
        [Obsolete("Methods specifying target device by deviceName are deprecated, please use MdsMovesenseDevice.SubscribeTimeAsync(...) instead.")]
        public async Task<IMdsSubscription> SubscribeTimeAsync(string deviceName, Action<TimeNotificationResult> notificationCallback)
        {
            var op = new ApiSubscription<TimeNotificationResult>(deviceName, TIME_SUBSCRIPTION_PATH);
            return await op.SubscribeAsync(notificationCallback).ConfigureAwait(false);
        }

        #endregion

    }
}
