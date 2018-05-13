using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    public class DataLoggerConfig
    {
        [JsonProperty(PropertyName ="config")]
        public  DataLoggerConfig.Config config;

        public DataLoggerConfig(DataLoggerConfig.Config config)
        {
            this.config = config;
        }

        public class Config
        {
            [JsonProperty(PropertyName = "dataEntries")]
            public DataEntries dataEntries;

            public Config(DataEntries dataEntries)
            {
                this.dataEntries = dataEntries;
            }
        }

        public class DataEntries
        {
            [JsonProperty(PropertyName = "dataEntry")]
            public DataEntry[] dataEntry;

            public DataEntries(DataEntry[] dataEntry)
            {
                this.dataEntry = dataEntry;
            }
        }


        public class DataEntry
        {
            [JsonProperty(PropertyName ="path")]
            public string path;

            public DataEntry(String path)
            {
                this.path = path;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataEntry de in config.dataEntries.dataEntry)
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                sb.Append(de.path);
            }

            return sb.ToString();
        }
    }
}
