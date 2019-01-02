using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MdsLibrary
{
    /// <summary>
    /// Listener for callbacks from MdsLib for device connections and disconnections
    /// </summary>
    public sealed partial class MdsConnectionListener
    {
        /// <summary>
        /// Event fires when a device connects to BLE
        /// </summary>
        public event EventHandler<MdsConnectionListenerBLEConnectedEventArgs> DeviceConnected;

        /// <summary>
        /// Event fires when connection has completed to WhiteBoard for a device to MdsLib
        /// </summary>
        public event EventHandler<MdsConnectionListenerEventArgs> DeviceConnectionComplete;

        /// <summary>
        /// Event fires when a device disconnects from MdsLib
        /// </summary>
        public event EventHandler<MdsConnectionListenerEventArgs> DeviceDisconnected;

        /// <summary>
        /// Event fires when MdsLib reports unexpected connection error
        /// </summary>
        public event EventHandler<MdsException> DeviceConnectionError;

        private static MdsConnectionListener instance = null;

        private static readonly object padlock = new object();


        /// <summary>
        /// Lookup of device serial number to UUID
        /// </summary>
        public Dictionary<string, string> MACAddressToSerialMapper;

        private MdsConnectionListener()
        {
            MACAddressToSerialMapper = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the current singleton MdsConnectionListener instance
        /// </summary>
        public static MdsConnectionListener Current
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MdsConnectionListener();
                    }
                    return instance;
                }
            }
        }
    }
}
