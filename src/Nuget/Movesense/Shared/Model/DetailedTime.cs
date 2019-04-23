using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Current time in number of microseconds since epoch 1.1.1970 (UTC).
    /// </summary>
    public class DetailedTime
    {
        /// <summary>
        /// Current time in number of microseconds since epoch 1.1.1970 (UTC). 
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        [JsonProperty("Content")]
        public Content Properties;

        /// <summary>
        /// Result content from query for GetLogEntries
        /// </summary>
        public class Content
        {
            /// <summary>
            /// Current time in number of microseconds since epoch 1.1.1970 (UTC). 
            /// If not explicitly set, contains number of seconds since reset.
            /// </summary>
            [JsonProperty("utcTime")]
            public long UtcTime;

            /// <summary>
            /// Local time of the device, milliseconds since device reset.
            /// </summary>
            [JsonProperty("relativeTime")]
            public uint RelativeTime;

            /// <summary>
            /// Tick rate of the device clock in ticks per second (Hz). This specifies the real resolution of the device clock.
            /// </summary>
            [JsonProperty("tickRate")]
            public uint TickRate;

            /// <summary>
            /// The nominal accuracy of the device clock in parts per million (ppm). 
            /// The actual accuracy depends on many things(device temperature, vibration environment etc.).
            /// </summary>
            [JsonProperty("accuracy")]
            public uint Accuracy;
        }
    }
}
