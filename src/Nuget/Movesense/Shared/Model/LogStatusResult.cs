using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// State of the DataLogger component
    /// </summary>
    public class LogStatusResult 
    {
        /// <summary>
        /// Current DataLogger status:
        /// 1:
        ///       name: 'DATALOGGER_INVALID'
        ///       description: Unknown or Invalid state prior to initialization or due to comm error.
        /// 2:
        ///       name: 'DATALOGGER_READY'
        ///       description: If we are ready for logging.
        /// 3:
        ///       name: 'DATALOGGER_LOGGING'
        ///       description: If we are logging data.
        /// </summary>
        [JsonProperty("Content")]
        public int LogStatus;
    }
}
