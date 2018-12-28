using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Movesense
{
    /// <summary>
    /// Contains serial number and MAC address of a connected sensor device. Returned from a call to MdsConnectAsync.
    /// Used throughout the API to specify the target device for an API call.
    /// </summary>
    public class MdsConnectionContext
    {
        /// <summary>
        /// Serial number of the connected device
        /// </summary>
        public string Serial { get; private set; }

        /// <summary>
        /// MAC address of the connected device
        /// </summary>
        public string MAC { get; private set; }

        /// <summary>
        /// Constructs an MdsConnectionContext
        /// </summary>
        /// <param name="serial">Serial number of the device</param>
        /// <param name="mac">MAC Address of ther device</param>
        public MdsConnectionContext(string serial, string mac)
        {
            this.Serial = serial;
            this.MAC = mac;
        }

    }
}
