using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    public class DeviceInfoResult
    {
        [JsonProperty("Content")]
        public Content mContent;

        public class Content
        {
            [JsonProperty("manufacturerName")]
            public string ManufacturerName;

            [JsonProperty("brandName")]
            public string BrandName;

            [JsonProperty("productName")]
            public string ProductName;

            [JsonProperty("variant")]
            public string Variant;

            [JsonProperty("design")]
            public string Design;

            [JsonProperty("hwCompatibilityId")]
            public string HwCompatibilityId;

            [JsonProperty("serial")]
            public string Serial;

            [JsonProperty("pcbaSerial")]
            public string PcbaSerial;

            [JsonProperty("sw")]
            public string Sw;

            [JsonProperty("hw")]
            public string Hw;

            [JsonProperty("additionalVersionInfo")]
            public VersionInfoArray VersionInfoArray;

            [JsonProperty("addressInfo")]
            public AddressInfo[] AddressInfo;

            [JsonProperty("apiLevel")]
            public string ApiLevel;
        }

        public class VersionInfoArray
        {
            [JsonProperty("versionInfo")]
            public VersionInfo[] mVersionInfo;

            public VersionInfo[] GetVersionInfo()
            {
                return mVersionInfo;
            }
        }

        public class VersionInfo
        {
            [JsonProperty("name")]
            public string Name;

            [JsonProperty("version")]
            public string Version;
        }

        public class AddressInfo
        {

            [JsonProperty("name")]
            public string Name;

            [JsonProperty("address")]
            public string Address;
        }

        public Content GetContent()
        {
            return mContent;
        }
    }
}
