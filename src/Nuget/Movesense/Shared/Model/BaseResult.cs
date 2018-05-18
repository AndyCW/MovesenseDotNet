using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Generic response class for retrieving non-strongly typed responses from MdsAPI
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// Data response from MdsAPI
        /// </summary>
        [JsonProperty("Content")]
        public string Content;
    }
}
