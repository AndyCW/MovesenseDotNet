using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// DataLogger configuration
    /// </summary>
    public class DataLoggerConfig
    {
        /// <summary>
        /// Contains the configuration of the Datalogger
        /// </summary>
        [JsonProperty(PropertyName ="config")]
        public  DataLoggerConfig.Config Configuration;

        /// <summary>
        /// Creates a DataLoggerConfig
        /// </summary>
        /// <param name="config">Configuration data</param>
        public DataLoggerConfig(DataLoggerConfig.Config config)
        {
            this.Configuration = config;
        }

        /// <summary>
        /// Configuration of the Datalogger
        /// </summary>
        public class Config
        {
            /// <summary>
            /// DataEntries object contining an array of structs containing wb-paths to the subscription of data to log.
            /// </summary>
            [JsonProperty(PropertyName = "dataEntries")]
            public DataEntries DataEntries;

            /// <summary>
            /// Creates the DataLogger Config object
            /// </summary>
            /// <param name="dataEntries">DataEntries object containing an array of structs containing wb-paths to the subscription of data to log.</param>
            public Config(DataEntries dataEntries)
            {
                this.DataEntries = dataEntries;
            }
        }

        /// <summary>
        /// Class container for an array of DataEntry objects.
        /// </summary>
        public class DataEntries
        {
            /// <summary>
            /// Array of DataEntry objects.
            /// </summary>
            [JsonProperty(PropertyName = "dataEntry")]
            public DataEntry[] DataEntry;

            /// <summary>
            /// Creates a DataEntries object
            /// </summary>
            /// <param name="dataEntry">Array of DataEntry objects</param>
            public DataEntries(DataEntry[] dataEntry)
            {
                this.DataEntry = dataEntry;
            }
        }

        /// <summary>
        /// Describes a data logger subscription 
        /// </summary>
        public class DataEntry
        {
            /// <summary>
            /// WB-path to the subscription of data to log.
            /// </summary>
            [JsonProperty(PropertyName ="path")]
            public string Path;

            /// <summary>
            /// Creates a DataEntry
            /// </summary>
            /// <param name="path">Wb-path to the subscription of data to log.</param>
            public DataEntry(String path)
            {
                this.Path = path;
            }
        }

        /// <summary>
        /// Provides a string representation of the DataEntries
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataEntry de in Configuration.DataEntries.DataEntry)
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                sb.Append(de.Path);
            }

            return sb.ToString();
        }
    }
}
