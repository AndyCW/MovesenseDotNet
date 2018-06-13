using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Current time in number of microseconds since epoch 1.1.1970 (UTC).
    /// </summary>
    public class TimeResult
    {
        /// <summary>
        /// Current time in number of microseconds since epoch 1.1.1970 (UTC). 
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        [JsonProperty("Content")]
        public long Time;
    }
}
