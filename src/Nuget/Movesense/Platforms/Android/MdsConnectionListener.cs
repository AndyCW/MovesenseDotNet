using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MdsLibrary
{
    /// <summary>
    /// Listener for callbacks from MdsLib for device connections and disconnections
    /// </summary>
    public sealed class MdsConnectionListener : Java.Lang.Object, Com.Movesense.Mds.IMdsConnectionListener
    {
        /// <summary>
        /// Event fires when a device connects to MdsLib
        /// </summary>
        public event EventHandler<MdsConnectionListenerEventArgs> DeviceConnected;

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

        private MdsConnectionListener()
        {
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

        /// <summary>
        /// Callback method that MdsLib calls when a device connects
        /// </summary>
        /// <param name="MACaddress"></param>
        public void OnConnect(string MACaddress)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnConnect callback called: MACaddress {MACaddress}");
            DeviceConnected?.Invoke(this, new MdsConnectionListenerEventArgs(MACaddress));
        }

        /// <summary>
        /// Callback method that MdsLib calls when connection of a device has completed
        /// </summary>
        /// <param name="MACaddress"></param>
        /// <param name="serial"></param>
        public void OnConnectionComplete(string MACaddress, string serial)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnConnectionComplete callback called: MACaddress {MACaddress} Serial {serial}");
            DeviceConnectionComplete?.Invoke(this, new MdsConnectionListenerEventArgs(MACaddress));
        }

        /// <summary>
        /// Callback method that MdsLib calls when a device disconnects from MdsLib
        /// </summary>
        /// <param name="MACaddress"></param>
        public void OnDisconnect(string MACaddress)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnDisconnect callback called: MACaddress {MACaddress}");
            DeviceDisconnected?.Invoke(this, new MdsConnectionListenerEventArgs(MACaddress));
        }

        /// <summary>
        /// Callback function that MdsLib calls when an exception is thrown during device connection
        /// </summary>
        /// <param name="e"></param>
        public void OnError(Com.Movesense.Mds.MdsException e)
        {
            // Unexpected device disconnections come in here
            if (e.Message.StartsWith("com.polidea.rxandroidble.exceptions.BleDisconnectedException"))
            {
                var msgParts = e.Message.Split(" ");
                string MACaddress = msgParts[msgParts.Length - 1];

                Debug.WriteLine($"DISCONNECT MdsConnectionListener OnError callback called for unintended disconnection: MACaddress {MACaddress}");
                DeviceDisconnected?.Invoke(this, new MdsConnectionListenerEventArgs(MACaddress));
            }
            else
            {
                // Other unexpected error
                Debug.WriteLine($"ERROR MdsConnectionListener OnError callback called for unexpected error: {e.ToString()}");
                DeviceConnectionError?.Invoke(this, new MdsException("MdsConnectionListener unexpected error", e));
            }
        }
    }

    /// <summary>
    /// Event args for MdsConnectionListener events
    /// </summary>
    public class MdsConnectionListenerEventArgs : EventArgs
    {
        /// <summary>
        /// MAC address of the device
        /// </summary>
        public string MACAddress { get; set; }

        /// <summary>
        /// Create event args for reporting connection events
        /// </summary>
        /// <param name="macAddress">MAC address of the device</param>
        public MdsConnectionListenerEventArgs(string macAddress)
        {
            MACAddress = macAddress;
        }
    }
}