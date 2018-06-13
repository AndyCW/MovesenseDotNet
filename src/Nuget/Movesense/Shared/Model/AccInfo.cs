using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    public class AccInfo
    {
        /// <summary>
        /// Container for the Accelerometer Configuration information
        /// </summary>
        [JsonProperty("Content")]
        public Content Data;

        /// <summary>
        /// Accelerometer Configuration information
        /// </summary>
        public class Content
        {
            /// <summary>
            /// Available sample rates for acceleration measurement.
            /// </summary>
            [JsonProperty("SampleRates")]
            public int[] SampleRates;

            /// <summary>
            /// Available ranges for acceleration measurement. For example range value 2 means the range is -2...+2 G.
            /// </summary>
            [JsonProperty("Ranges")]
            public int[] Ranges;
        }
    }
}
