using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Response from command to create a new Log entry resource
    /// </summary>
    public class CreateLogResult
    {
        /// <summary>
        /// ID of the new Logbook entry resource
        /// </summary>
        [JsonProperty("Content")]
        public int LogId;
    }
}
