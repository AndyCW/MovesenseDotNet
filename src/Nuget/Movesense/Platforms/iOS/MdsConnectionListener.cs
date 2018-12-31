#if __IOS__
using Foundation;
using Movesense;
using Plugin.Movesense;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MdsLibrary
{
    /// <summary>
    /// Listener for callbacks from MdsLib for device connections and disconnections
    /// </summary>

    public sealed class MdsConnectionListener
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

        private static bool mIsListening = false;

        private TaskCompletionSource<bool> setuplistenertcs;


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

        /// <summary>
        /// Setup the (Dis)Connection listener for iOS devices
        /// </summary>
        /// <returns></returns>
        public Task<bool> EnsureInitializedAsync()
        {
            setuplistenertcs = new TaskCompletionSource<bool>();

            if (!mIsListening)
            {
                // Setup the connection listener
                MDSResponseBlock responseBlock = new MDSResponseBlock((arg0) => MdsConnectionListener.Current.OnListenerSetupCompleted(arg0));
                MDSEventBlock eventBlock = (MDSEvent arg0) => MdsConnectionListener.Current.OnDeviceConnectionEvent(arg0);
                ((MDSWrapper)(CrossMovesense.Current.MdsInstance)).DoSubscribe("MDS/ConnectedDevices", new Foundation.NSDictionary(), responseBlock, eventBlock);
            }
            else
            {
                // Complete immediately if we've already done this
                setuplistenertcs?.TrySetResult(true);
            }

            return setuplistenertcs.Task;
        }

        private void OnListenerSetupCompleted(MDSResponse response)
        {
            if (response.StatusCode == 200)
            {
                System.Diagnostics.Debug.WriteLine("Listening connected devices: " + response.Description);
                mIsListening = true;
                setuplistenertcs?.TrySetResult(true);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to start listening connected devices: " + response.Description);
                mIsListening = false;
                setuplistenertcs?.TrySetResult(false);
            }
        }

        /// <summary>
        /// Callback method that MdsLib calls when a device connects or disconnects
        /// </summary>
        /// <param name="mdsevent">details of device connection/disconnection</param>
        public void OnDeviceConnectionEvent(MDSEvent mdsevent)
        {
            var method = ((NSString)mdsevent.BodyDictionary.ValueForKey(new NSString("Method")));
            if (method == new NSString("POST"))
            {
                // Device connected
                var bodyDict = (NSDictionary)mdsevent.BodyDictionary.ValueForKey(new NSString("Body"));
                var serial = ((NSString)bodyDict.ValueForKey(new NSString("Serial"))).ToString();
                var connDict = (NSDictionary)bodyDict.ValueForKey(new NSString("Connection"));
                var uuid = ((NSString)connDict.ValueForKey(new NSString("UUID"))).ToString();

                this.MACAddressToSerialMapper.TryAdd(uuid, serial);
                Debug.WriteLine($"MdsConnectionListener OnDeviceConnectionEvent CONNECTED: Serial {serial}");
                DeviceConnected?.Invoke(this, new MdsConnectionListenerBLEConnectedEventArgs(uuid));
                DeviceConnectionComplete?.Invoke(this, new MdsConnectionListenerEventArgs(serial));
            }
            else if (method == new NSString("DEL"))
            {
                // Device disconnected
                var bodyDict = (NSDictionary)mdsevent.BodyDictionary.ValueForKey(new NSString("Body"));
                var serial = ((NSString)bodyDict.ValueForKey(new NSString("Serial"))).ToString();

                Debug.WriteLine($"MdsConnectionListener OnDeviceConnectionEvent DISCONNECTED: Serial {serial}");
                DeviceDisconnected?.Invoke(this, new MdsConnectionListenerEventArgs(serial));
            }
            else
            {
                throw new MdsException($"OnDeviceConnectionEvent unexpected method: {method}");
            }
        }
    }
}
#endif