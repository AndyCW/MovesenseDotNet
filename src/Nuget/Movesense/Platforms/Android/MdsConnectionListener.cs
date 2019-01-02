#if __ANDROID__
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MdsLibrary
{
    /// <summary>
    /// Listener for callbacks from MdsLib for device connections and disconnections
    /// </summary>
    public sealed partial class MdsConnectionListener : Java.Lang.Object, Com.Movesense.Mds.IMdsConnectionListener
    {

        /// <summary>
        /// Callback method that MdsLib calls when a device connects
        /// </summary>
        /// <param name="MACaddress"></param>
        public void OnConnect(string MACaddress)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnConnect callback called: MACaddress {MACaddress}");
            DeviceConnected?.Invoke(this, new MdsConnectionListenerBLEConnectedEventArgs(MACaddress));
        }

        /// <summary>
        /// Callback method that MdsLib calls when connection of a device has completed
        /// </summary>
        /// <param name="MACaddress"></param>
        /// <param name="serial"></param>
        public void OnConnectionComplete(string MACaddress, string serial)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnConnectionComplete callback called: MACaddress {MACaddress} Serial {serial}");
            this.MACAddressToSerialMapper.TryAdd(MACaddress, serial);

            DeviceConnectionComplete?.Invoke(this, new MdsConnectionListenerEventArgs(serial));
        }

        /// <summary>
        /// Callback method that MdsLib calls when a device disconnects from MdsLib
        /// </summary>
        /// <param name="MACaddress"></param>
        public void OnDisconnect(string MACaddress)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnDisconnect callback called: MACaddress {MACaddress}");
            var serial = string.Empty;
            this.MACAddressToSerialMapper.TryGetValue(MACaddress, out serial);
            DeviceDisconnected?.Invoke(this, new MdsConnectionListenerEventArgs(serial));
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
                var serial = string.Empty;
                this.MACAddressToSerialMapper.TryGetValue(MACaddress, out serial);
                DeviceDisconnected?.Invoke(this, new MdsConnectionListenerEventArgs(serial));
            }
            else
            {
                // Other unexpected error
                Debug.WriteLine($"ERROR MdsConnectionListener OnError callback called for unexpected error: {e.ToString()}");
                DeviceConnectionError?.Invoke(this, new MdsException("MdsConnectionListener unexpected error", e));
            }
        }
    }
}
#endif