using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary
{
    /// <summary>
    /// Event args for MdsConnectionListener events, for reporting device connection to BLE
    /// </summary>
    public class MdsConnectionListenerBLEConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// MAC Address of the device
        /// </summary>
        public string MACAddress { get; set; }

        /// <summary>
        /// Create event args for reporting connection events
        /// </summary>
        /// <param name="macAddress">MAC Address of the device</param>
        public MdsConnectionListenerBLEConnectedEventArgs(string macAddress)
        {
            MACAddress = macAddress;
        }
    }
}
