using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Energy level response
    /// </summary>
    public class BatteryResult
    {
        /// <summary>
        /// Estimated battery charge percent
        /// </summary>
        [JsonProperty("Content")]
        public int ChargePercent;
    }
}
