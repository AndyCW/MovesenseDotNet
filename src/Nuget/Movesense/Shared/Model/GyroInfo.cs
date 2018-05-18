using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Gyroscope measurement configuration
    /// </summary>
    public class GyroInfo
    {
        /// <summary>
        /// Container for Gyroscope measurement configuration
        /// </summary>
        [JsonProperty("Content")]
        public Content Info;

        /// <summary>
        /// Gyroscope measurement configuration
        /// </summary>
        public class Content
        {
            /// <summary>
            /// Available sample rates for gyroscope measurement.
            /// </summary>
            [JsonProperty("SampleRates")]
            public int[] SampleRates;

            /// <summary>
            /// Available ranges for angular range measurement rate. For example range value 500 means the range is -500...+500 dps.
              /// </summary>
            [JsonProperty("Ranges")]
            public int[] Ranges;
        }
    }
}

