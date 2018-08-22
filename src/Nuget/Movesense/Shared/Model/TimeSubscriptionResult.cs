using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Time notification returned as Json from subscription to {device}/Time
    /// </summary>
    public class TimeNotificationResult
    {
        /// <summary>
        /// Current device time in microseconds since epoch 1.1.1970 (UTC).
        /// </summary>
        [JsonProperty("Body")]
        public long Time { get; set; }

        /// <summary>
        /// The subscription URI, example "174430000024/Time"
        /// </summary>
        [JsonProperty("Uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Method used for the subscription notification - will be "POST"
        /// </summary>
        [JsonProperty("Method")]
        public string Method { get; set; }
    }
}
