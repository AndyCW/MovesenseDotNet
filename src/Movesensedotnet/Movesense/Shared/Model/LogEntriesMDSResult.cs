using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Result from query for GetLogEntriesMDS
    /// </summary>
    public class LogEntriesMDSResult
    {
        /// <summary>
        /// Array of Zero or more log entries describing log contents. 
        /// Zero entries are received if log iteration has completed.
        /// </summary>
        [JsonProperty("elements")]
        public LogEntry[] Elements;

        /// <summary>
        /// Log Entry data
        /// </summary>
        public class LogEntry
        {
            /// <summary>
            /// Id of the log entry.
            /// </summary>
            [JsonProperty("Id")]
            public int Id;

            /// <summary>
            /// Timestamp of last modification to entry in seconds after 0:00 Jan 1st 1970 (UTC) without leap seconds.
            /// </summary>
            [JsonProperty("ModificationTimestamp")]
            public uint ModificationTimestamp;
        }
    }
}
