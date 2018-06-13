using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Available sample rates and scales for magnetometer measurement.
    /// </summary>
    public class MagnInfo
    {
        /// <summary>
        /// Info on available sample rates and scales for magnetometer measurement.
        /// </summary>
        [JsonProperty("Content")]
        public Content Info;

        /// <summary>
        /// Available sample rates and scales for magnetometer measurement.
        /// </summary>
        public class Content
        {
            /// <summary>
            /// Available sample rates for magnetometer measurement.
            /// </summary>
            [JsonProperty("SampleRates")]
            public int[] SampleRates;

            /// <summary>
            /// Available scales for magnetometer measurement. For example scale value 400 means the range is -400...+400 µT(microtesla).
            /// </summary>
            [JsonProperty("Scale")]
            public int[] Scale;
        }
    }
}

