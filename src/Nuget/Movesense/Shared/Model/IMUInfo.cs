using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Supported IMU sample rates and ranges.
    /// </summary>
    public class IMUInfo
    {
        /// <summary>
        /// Supported IMU sample rates and ranges.
        /// </summary>
        [JsonProperty("Content")]
        public Content Info;

        /// <summary>
        /// Supported sample rates and ranges.
        /// </summary>
        public class Content
        {
            /// <summary>
            /// Available sample rates for IMU measurement.
            /// </summary>
            [JsonProperty("SampleRates")]
            public int[] SampleRates;

            /// <summary>
            /// Available ranges for acceleration measurement. For example range value 2 means the range is -2...+2 G.
              /// </summary>
            [JsonProperty("AccRanges")]
            public int[] AccRanges;

            /// <summary>
            /// Available ranges for angular range measurement rate. For example range value 500 means the range is -500...+500 dps.
              /// </summary>
            [JsonProperty("GyroRanges")]
            public int[] GyroRanges;

            /// <summary>
            /// Available scales for magnetometer measurement. For example scale value 400 means the range is -400...+400 µT(microtesla).
            /// </summary>
            [JsonProperty("MagnRanges")]
            public int[] MagnRanges;
        }
    }
}
