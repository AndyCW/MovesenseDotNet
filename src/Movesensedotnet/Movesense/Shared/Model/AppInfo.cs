using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Class representation of the JSON returned by a Get of Application info
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// Device Information query response
        /// </summary>
        [JsonProperty("Content")]
        public AppInfoData Data;

        /// <summary>
        /// App Information 
        /// </summary>
        public class AppInfoData
        {
            /// <summary>
            /// Application name, example: 'Running app'
            /// </summary>
            [JsonProperty("name")]
            public string ApplicationName;

            /// <summary>
            /// Application version, example '1.0.0'
            /// </summary>
            [JsonProperty("version")]
            public string ApplicationVersion;

            /// <summary>
            /// Application provider, example 'Suunto'
            /// </summary>
            [JsonProperty("company")]
            public string ApplicationProvider;

            /// <summary>
            /// Array of ModuleStatus, giving modules names and status
            /// </summary>
            [JsonProperty("modules")]
            public ModulesStatusArray Modules;
        }

        /// <summary>
        /// Version Info array
        /// </summary>
        public class ModulesStatusArray
        {
            /// <summary>
            /// Version Info array
            /// </summary>
            [JsonProperty("data")]
            public ModuleStatus[] ModulesStatus;
        }

        /// <summary>
        /// Status of an installed module
        /// </summary>
        public class ModuleStatus
        {
            /// <summary>
            /// Module name, example 'Eeprom'
            /// </summary>
            [JsonProperty("name")]
            public string ModuleName;

            /// <summary>
            /// Module status
            /// </summary>
            [JsonProperty("enabled")]
            public bool IsEnabled;
        }
    }
}
