using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Contains response to a device information query
    /// </summary>
    public class DeviceInfoResult
    {
        /// <summary>
        /// Device Information query response
        /// </summary>
        [JsonProperty("Content")]
        public DeviceInfoData DeviceInfo;

        /// <summary>
        /// Device Information 
        /// </summary>
        public class DeviceInfoData
        {
            /// <summary>
            /// Original Equipment Manufacturer (OEM)
            /// </summary>
            [JsonProperty("manufacturerName")]
            public string ManufacturerName;

            /// <summary>
            /// Brand name of the device
            /// </summary>
            [JsonProperty("brandName")]
            public string BrandName;

            /// <summary>
            /// Official product name visible to the consumers
            /// </summary>
            [JsonProperty("productName")]
            public string ProductName;

            /// <summary>
            /// Internal SW variant name
            /// </summary>
            [JsonProperty("variant")]
            public string Variant;

            /// <summary>
            /// Product design name
            /// </summary>
            [JsonProperty("design")]
            public string Design;

            /// <summary>
            /// Identifier defining which firmware can be installed on the device.
            /// This ID together with the variant name is used to check/fetch the latest FW.Different PCBAs which can run the 
            /// same Firmware, have the same HwCompatibilityId.
            /// </summary>
            [JsonProperty("hwCompatibilityId")]
            public string HwCompatibilityId;

            /// <summary>
            /// Globally unique device serial number (12 digit)
            /// </summary>
            [JsonProperty("serial")]
            public string Serial;

            /// <summary>
            /// Globally unique PCBA assemply identifier (this can be OEM specific).
            /// </summary>
            [JsonProperty("pcbaSerial")]
            public string PcbaSerial;

            /// <summary>
            /// Version number of the firmware.
            /// </summary>
            [JsonProperty("sw")]
            public string Sw;

            /// <summary>
            /// Identifier for the HW configuration. Everytime there are changes in components/PCB, this ID is updated.
            /// </summary>
            [JsonProperty("hw")]
            public string Hw;

            /// <summary>
            /// Array of VersionInfo
            /// </summary>
            [JsonProperty("additionalVersionInfo")]
            public VersionInfoArray VersionInfoArray;

            /// <summary>
            /// Array of structs containing pairs of address type identifier and address.
            /// </summary>
            [JsonProperty("addressInfo")]
            public AddressInfo[] AddressInfo;

            /// <summary>
            /// Movesense-api level. This is defined by single number.
            /// </summary>
            [JsonProperty("apiLevel")]
            public string ApiLevel;
        }

        /// <summary>
        /// Version Info array
        /// </summary>
        public class VersionInfoArray
        {
            /// <summary>
            /// Version Info array
            /// </summary>
            [JsonProperty("versionInfo")]
            public VersionInfo[] VersionInfo;
        }

        /// <summary>
        /// Version info
        /// </summary>
        public class VersionInfo
        {
            /// <summary>
            /// Version name
            /// </summary>
            [JsonProperty("name")]
            public string Name;

            /// <summary>
            /// Version
            /// </summary>
            [JsonProperty("version")]
            public string Version;
        }

        /// <summary>
        /// Network address info
        /// </summary>
        public class AddressInfo
        {
            /// <summary>
            /// Address type (BLE, WIFI)
            /// </summary>
            [JsonProperty("name")]
            public string Name;

            /// <summary>
            /// Address value, for example 80-1F-02-4E-F1-70
            /// </summary>
            [JsonProperty("address")]
            public string Address;
        }
    }
}
