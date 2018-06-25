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
        public event EventHandler<MdsConnectionListenerEventArgs> Connect;

        /// <summary>
        /// Event fires when connection has completed for a device to MdsLib
        /// </summary>
        public event EventHandler<MdsConnectionListenerEventArgs> ConnectionComplete;

        /// <summary>
        /// Event fires when a device disconnects from MdsLib
        /// </summary>
        public event EventHandler<MdsConnectionListenerEventArgs> Disconnect;

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
            Connect?.Invoke(this, new MdsConnectionListenerEventArgs(MACaddress));
        }

        /// <summary>
        /// Callback method that MdsLib calls when connection of a device has completed
        /// </summary>
        /// <param name="MACaddress"></param>
        /// <param name="serial"></param>
        public void OnConnectionComplete(string MACaddress, string serial)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnConnectionComplete callback called: MACaddress {MACaddress} Serial {serial}");
            ConnectionComplete?.Invoke(this, new MdsConnectionListenerEventArgs(MACaddress));
        }

        /// <summary>
        /// Callback method that MdsLib calls when a device disconnects from MdsLib
        /// </summary>
        /// <param name="MACaddress"></param>
        public void OnDisconnect(string MACaddress)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnDisconnect callback called: MACaddress {MACaddress}");
            Disconnect?.Invoke(this, new MdsConnectionListenerEventArgs(MACaddress));
        }

        /// <summary>
        /// Callback function that MdsLib calls when an exception is thrown during device connection
        /// </summary>
        /// <param name="e"></param>
        public void OnError(Com.Movesense.Mds.MdsException e)
        {
            throw new MdsException(e.ToString(), e);
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

        public MdsConnectionListenerEventArgs(string macAddress)
        {
            MACAddress = macAddress;
        }
    }
}