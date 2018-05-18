using MdsLibrary.Api;
using MdsLibrary.Model;
using Plugin.Movesense.Api;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Plugin.Movesense
{
    public partial class MovesenseImplementation
    {
        private static readonly string LOGGER_ENTRIES_PATH = "/Mem/Logbook/Entries";
        private static readonly string ACC_INFO_PATH = "/Meas/Acc/Info";
        private static readonly string BATTERY_PATH = "/System/Energy/Level";
        private static readonly string LOGBOOK_DATA_PATH = "/Mem/Logbook/byId/{0}/Data";
        private static readonly string URI_MDS_LOGBOOK_DATA = "/Mem/Logbook/byId/{0}/Descriptors";
        private static readonly string INFO_PATH = "/Info";
        private static readonly string GYRO_INFO_PATH = "/Meas/Gyro/Info";
        private static readonly string IMU_INFO_PATH = "/Meas/IMU/Info";
        private static readonly string LED_PATH = "/Component/Leds/{0}";
        private static readonly string LEDS_PATH = "/Component/Leds";
        private static readonly string DATALOGGER_STATE_PATH = "/Mem/DataLogger/State/";
        private static readonly string MAG_INFO_PATH = "/Meas/Magn/Info";
        private static readonly string TIME_PATH = "/Time";
        private static readonly string DATALOGGER_CONFIG_PATH = "/Mem/DataLogger/Config/";
        private static readonly string ACCELEROMETER_PATH = "Meas/Acc/";
        private static readonly string GYROMETER_PATH = "Meas/Gyro/";
        private static readonly string MAGNETOMETER_PATH = "Meas/Magn/";
        private static readonly string IMU6_PATH = "Meas/IMU6/";
        private static readonly string IMU9_PATH = "Meas/IMU9/";

        private const int DEFAULT_SAMPLE_RATE = 26;

        /// <summary>
        /// Create a new logbook entry resource (increment log Id). Returns the new log Id.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        public async Task<CreateLogResult> CreateLogEntryAsync(string deviceName)
        {
            var op = new ApiCallAsync<CreateLogResult>(deviceName, MdsOp.POST, LOGGER_ENTRIES_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get details of Logbook entries
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        public async Task<LogEntriesResult> GetLogEntriesAsync(string deviceName)
        {
            var op = new ApiCallAsync<LogEntriesResult>(deviceName, MdsOp.GET, LOGGER_ENTRIES_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Delete all the Logbook entries
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        public async Task DeleteLogEntriesAsync(string deviceName)
        {
            var op = new ApiCallAsync(deviceName, MdsOp.DELETE, LOGGER_ENTRIES_PATH);
            await op.CallAsync();
        }


        /// <summary>
        /// Get Accelerometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        public async Task<AccInfo> GetAccInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<AccInfo>(deviceName, MdsOp.GET, ACC_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Magnetometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        public async Task<MagnInfo> GetMagInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<MagnInfo>(deviceName, MdsOp.GET, MAG_INFO_PATH);
            return await op.CallAsync();
        }


        /// <summary>
        /// Get Battery level, CallAsync returns BatteryResult
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
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
        public async Task<BaseResult> GetLogbookDataAsync(string deviceName, int logId)
        {
            string datapath = String.Format(LOGBOOK_DATA_PATH, logId);
            var op = new ApiCallAsync<BaseResult>(deviceName, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Descriptors for a Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Logbook entry to get</param>
        public async Task<BaseResult> GetLogbookDescriptorsAsync(string deviceName, int logId)
        {
            string datapath = String.Format(URI_MDS_LOGBOOK_DATA, logId);
            var op = new ApiCallAsync<BaseResult>(deviceName, MdsOp.GET, datapath);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get device info
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        public async Task<DeviceInfoResult> GetDeviceInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<DeviceInfoResult>(deviceName, MdsOp.GET, INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Gyrometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        public async Task<GyroInfo> GetGyroInfoAsync(string deviceName)
        {
            var op = new ApiCallAsync<GyroInfo>(deviceName, MdsOp.GET, GYRO_INFO_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get IMU configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
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
        public async Task<LedsResult> GetLedsStateAsync(string deviceName)
        {
            var op = new ApiCallAsync<LedsResult>(deviceName, MdsOp.GET, LEDS_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Get Logger status, CallAsync returns LogStatusResult object
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
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
        /// Gets current time in number of microseconds since epoch 1.1.1970 (UTC).
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        public async Task<TimeResult> GetTimeAsync(string deviceName)
        {
            var op = new ApiCallAsync<TimeResult>(deviceName, MdsOp.GET, TIME_PATH);
            return await op.CallAsync();
        }

        /// <summary>
        /// Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC).
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
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
        public async Task<IMdsSubscription> SubscribeAccelerometerAsync(string deviceName, Action<AccData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            var op = new ApiSubscription<AccData>(deviceName, ACCELEROMETER_PATH, sampleRate);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic Gyrometer data
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the GyroData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeGyrometerAsync(string deviceName, Action<GyroData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            var op = new ApiSubscription<GyroData>(deviceName, GYROMETER_PATH, sampleRate);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic Magnetometer data measurements
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the MagnData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeMagnetometerAsync(string deviceName, Action<MagnData> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            var op = new ApiSubscription<MagnData>(deviceName, MAGNETOMETER_PATH, sampleRate);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic 6-axis IMU measurements (Acc + Gyro).
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the IMU6Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeIMU6Async(string deviceName, Action<IMU6Data> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            var op = new ApiSubscription<IMU6Data>(deviceName, IMU6_PATH, sampleRate);
            return await op.SubscribeAsync(notificationCallback);
        }

        /// <summary>
        /// Subscribe to periodic 9-axis IMU measurements.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the IMU9Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public async Task<IMdsSubscription> SubscribeIMU9Async(string deviceName, Action<IMU9Data> notificationCallback, int sampleRate = DEFAULT_SAMPLE_RATE)
        {
            var op = new ApiSubscription<IMU9Data>(deviceName, IMU9_PATH, sampleRate);
            return await op.SubscribeAsync(notificationCallback);
        }
    }
}
