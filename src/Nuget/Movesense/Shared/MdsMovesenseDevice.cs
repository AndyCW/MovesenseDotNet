using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using MdsLibrary;
using MdsLibrary.Api;
using MdsLibrary.Model;
using Plugin.Movesense.Api;

namespace Plugin.Movesense
{
    /// <summary>
    /// Implements all the operations on a Movesense Device.
    /// Contains serial number and MAC address of a connected sensor device. 
    /// Returned from a call to MdsConnectAsync.
    /// </summary>
    public class MdsMovesenseDevice : IMovesenseDevice
    {
        private static readonly string LOGBOOK_ENTRIES_PATH = "/Mem/Logbook/Entries";
        private static readonly string ACC_INFO_PATH = "/Meas/Acc/Info";
        private static readonly string APP_INFO_PATH = "/Info/App";
        private static readonly string BATTERY_PATH = "/System/Energy/Level";
        private static readonly string DETAILED_TIME_PATH = "/Time/Detailed";
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
        /// Serial number of the connected device
        /// </summary>
        public string Serial { get; private set; }

        /// <summary>
        /// Unique Id of the connected device
        /// </summary>
        public Guid Uuid { get; private set; }

        /// <summary>
        /// Constructs an MdsConnectionContext
        /// </summary>
        /// <param name="serial">Serial number of the device</param>
        /// <param name="uniqueIdentifier">MAC Address of ther device</param>
        public MdsMovesenseDevice(string serial, Guid uniqueIdentifier)
        {
            this.Serial = serial;
            this.Uuid = uniqueIdentifier;
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <returns>null</returns>
        public async Task<object> DisconnectMdsAsync()
        {
            var movesense = Plugin.Movesense.CrossMovesense.Current;
            return await movesense.DisconnectMdsAsync(this);
        }

        #region Movesense.NET function methods

        /// <summary>
        /// Create a new logbook entry resource (increment log Id). Returns the new log Id.
        /// </summary>
        public async Task<CreateLogResult> CreateLogEntryAsync()
        {
            var op = new ApiCallAsync<CreateLogResult>(this, MdsOp.POST, LOGBOOK_ENTRIES_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://{serial}/Mem/Logbook/Entries REST endpoint
        /// </summary>
        public async Task<LogEntriesResult> GetLogEntriesAsync()
        {
            var op = new ApiCallAsync<LogEntriesResult>(this, MdsOp.GET, LOGBOOK_ENTRIES_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://MDS/Logbook/{serial}>/Entries" REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.
        /// </summary>
        public async Task<LogEntriesMDSResult> GetLogEntriesJsonAsync()
        {
            var op = new ApiCallAsync<LogEntriesMDSResult>(this, MdsOp.GET, MDS_LOGBOOK_ENTRIES_PATH, null, MDS_LOGBOOK_PREFIX);
            return await op.CallAsync();
        }

        /// <summary>
        /// Delete all the Logbook entries
        /// </summary>
        public async Task DeleteLogEntriesAsync()
        {
            var op = new ApiCallAsync(this, MdsOp.DELETE, LOGBOOK_ENTRIES_PATH);
            await op.CallAsync();
        }


        /// <summary>
        /// Get Accelerometer configuration
        /// </summary>
        public async Task<AccInfo> GetAccInfoAsync()
        {
            var op = new ApiCallAsync<AccInfo>(this, MdsOp.GET, ACC_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Magnetometer configuration
        /// </summary>
        public async Task<MagnInfo> GetMagInfoAsync()
        {
            var op = new ApiCallAsync<MagnInfo>(this, MdsOp.GET, MAG_INFO_PATH);
            return await op.CallAsync();
        }


        /// <summary>
        /// Get Battery level, CallAsync returns BatteryResult
        /// </summary>
        public async Task<BatteryResult> GetBatteryLevelAsync()
        {
            var op = new ApiCallAsync<BatteryResult>(this, MdsOp.GET, BATTERY_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get data from a Logbook entry
        /// </summary>
        /// <param name="logId">Number of the entry to get</param>
        public async Task<string> GetLogbookDataAsync(int logId)
        {
            string datapath = String.Format(LOGBOOK_DATA_PATH, logId);
            var op = new ApiCallAsync<string>(this, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get data from a Logbook entry as Json by accessing the suunto://MDS/Logbook/{serial}>/ByID/{ID}/Data REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.  
        /// </summary>
        /// <param name="logId">Number of the entry to get</param>
        public async Task<string> GetLogbookDataJsonAsync(int logId)
        {
            string datapath = String.Format(MDS_LOGBOOK_DATA_PATH, logId);
            var op = new ApiCallAsync<string>(this, MdsOp.GET, datapath, null, MDS_LOGBOOK_PREFIX);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Descriptors for a Logbook entry
        /// </summary>
        /// <param name="logId">Logbook entry to get</param>
        public async Task<BaseResult> GetLogbookDescriptorsAsync(int logId)
        {
            string datapath = String.Format(LOGBOOK_DATA, logId);
            var op = new ApiCallAsync<BaseResult>(this, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get info on the app running on the device
        /// </summary>
        public async Task<AppInfo> GetAppInfoAsync()
        {
            var op = new ApiCallAsync<AppInfo>(this, MdsOp.GET, APP_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get device info
        /// </summary>
        public async Task<DetailedTime> GetDetailedTimeAsync()
        {
            var op = new ApiCallAsync<DetailedTime>(this, MdsOp.GET, DETAILED_TIME_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get device info
        /// </summary>
        public async Task<DeviceInfoResult> GetDeviceInfoAsync()
        {
            var op = new ApiCallAsync<DeviceInfoResult>(this, MdsOp.GET, INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Gyrometer configuration
        /// </summary>
        public async Task<GyroInfo> GetGyroInfoAsync()
        {
            var op = new ApiCallAsync<GyroInfo>(this, MdsOp.GET, GYRO_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get IMU configuration
        /// </summary>
        public async Task<IMUInfo> GetIMUInfoAsync()
        {
            var op = new ApiCallAsync<IMUInfo>(this, MdsOp.GET, IMU_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get LedState for an LED
        /// </summary>
        /// <param name="ledIndex">Number of the Led</param>
        public async Task<LedState> GetLedStateAsync(int ledIndex = 0)
        {
            string datapath = String.Format(LED_PATH, ledIndex);
            var op = new ApiCallAsync<LedState>(this, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Sets state of an LED
        /// </summary>
        /// <param name="ledIndex">Index of the Led - use 0 for standard Movesense sensor</param>
        /// <param name="ledOn">Set on or off</param>
        /// <param name="ledColor">[optional]value from LedColor enumeration - default is LedColor.Red</param>
        public async Task SetLedStateAsync(int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red)
        {
            string datapath = String.Format(LED_PATH, ledIndex);
            string led_On_Body = $"{{ \"LedState\": {{ \"IsOn\": true, \"LedColor\": {(int)ledColor}}} }}";
            string led_Off_Body = @"{ ""LedState"": { ""IsOn"": false, ""LedColor"": 0} }";
            var op = new ApiCallAsync<LedState>(this, MdsOp.PUT, datapath, ledOn ? led_On_Body : led_Off_Body);
            await op.CallAsync();
        }

        /// <summary>
        /// Get state of all Leds in the system
        /// </summary>
        public async Task<LedsResult> GetLedsStateAsync()
        {
            var op = new ApiCallAsync<LedsResult>(this, MdsOp.GET, LEDS_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Logger status, CallAsync returns LogStatusResult object
        /// </summary>
        public async Task<LogStatusResult> GetLoggerStatusAsync()
        {
            var op = new ApiCallAsync<LogStatusResult>(this, MdsOp.GET, DATALOGGER_STATE_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Set state of the Datalogger
        /// </summary>
        /// <param name="start">Set true to start the datalogger, false to stop</param>
        public async Task SetLoggerStatusAsync(bool start)
        {
            string LOG_ON = "{\"newState\":3}";
            string LOG_OFF = "{\"newState\":2}";
            var op = new ApiCallAsync(this, MdsOp.PUT, DATALOGGER_STATE_PATH, start ? LOG_ON : LOG_OFF);
            await op.CallAsync();
        }

        /// <summary>
        /// Set configuration for the Datalogger - ONLY sets IMU9
        /// </summary>
        /// <param name="freq">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task SetupLoggerAsync(int freq = 26)
        {
            DataLoggerConfig.DataEntry[] entries = {
                new DataLoggerConfig.DataEntry("/Meas/IMU9/" + freq)
            };

            DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
            string jsonConfig = Newtonsoft.Json.JsonConvert.SerializeObject(config);

            var op = new ApiCallAsync(this, MdsOp.PUT, DATALOGGER_CONFIG_PATH, jsonConfig);
            await op.CallAsync();
        }

        /// <summary>
        /// Set configuration for the Datalogger
        /// </summary>
        /// <param name="dataLoggerConfig">Configuration to apply to the DataLogger. Config is an array of structs containing paths to the subscription of data to log.
        /// For example:             
        /// DataLoggerConfig.DataEntry[] entries = { new DataLoggerConfig.DataEntry("/Meas/IMU9/52") };
        /// DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
        /// </param>
        public async Task SetLoggerConfigAsync(DataLoggerConfig dataLoggerConfig)
        {
            string jsonConfig = Newtonsoft.Json.JsonConvert.SerializeObject(dataLoggerConfig);

            var op = new ApiCallAsync(this, MdsOp.PUT, DATALOGGER_CONFIG_PATH, jsonConfig);
            await op.CallAsync();
        }

        /// <summary>
        /// Gets current time in number of microseconds since epoch 1.1.1970 (UTC).
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        public async Task<TimeResult> GetTimeAsync()
        {
            var op = new ApiCallAsync<TimeResult>(this, MdsOp.GET, TIME_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC).
        /// </summary>
        public async Task SetTimeAsync()
        {
            long timems = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            string requestBody = $"{{\"value\":{timems * 1000}}}";
            Debug.WriteLine($"INFO SetTimeAsync TIME {requestBody}");
            var op = new ApiCallAsync<TimeResult>(this, MdsOp.PUT, TIME_PATH, requestBody);
            await op.CallAsync();
        }

        /// <summary>
        /// Subscribe to periodic linear acceleration measurements.
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the AccData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeAccelerometerAsync(Action<AccData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(ACCELEROMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<AccData>(this, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic Gyrometer data
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the GyroData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeGyrometerAsync(Action<GyroData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(GYROMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<GyroData>(this, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic 6-axis IMU measurements (Acc + Gyro).
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the IMU6Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeIMU6Async(Action<IMU6Data> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(IMU6_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<IMU6Data>(this, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic 9-axis IMU measurements.
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the IMU9Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeIMU9Async(Action<IMU9Data> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(IMU9_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<IMU9Data>(this, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic Magnetometer data measurements
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the MagnData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeMagnetometerAsync(Action<MagnData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            string datapath = String.Format(MAGNETOMETER_SUBSCRIPTION_PATH, sampleRate);
            var op = new ApiSubscription<MagnData>(this, datapath);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to device time notifications
        /// </summary>
        /// <param name="notificationCallback">Callback function to receive the time data</param>
        public async Task<IMdsSubscription> SubscribeTimeAsync(Action<TimeNotificationResult> notificationCallback)
        {
            var op = new ApiSubscription<TimeNotificationResult>(this, TIME_SUBSCRIPTION_PATH);
            return await op.SubscribeAsync(notificationCallback);
        }

        #endregion
    }
}
