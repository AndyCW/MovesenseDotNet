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
        /// Event fires when connection has completed for a device to MdsLib
        /// </summary>
        public event EventHandler<MdsConnectionListenerEventArgs> ConnectionComplete;

        /// <summary>
        /// Event fires when a device disconnects from MdsLib
        /// </summary>
        public event EventHandler<MdsConnectionListenerEventArgs> Disconnect;

        private static MdsConnectionListener instance = null;

        private static readonly object padlock = new object();

        private static bool mIsListening = false;

        private TaskCompletionSource<bool> setuplistenertcs;


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
        /// Setup the (Dis)Connection listener for iOS devices
        /// </summary>
        /// <returns></returns>
        public Task<bool> EnsureInitializedAsync()
        {
            setuplistenertcs = new TaskCompletionSource<bool>();

            if (!mIsListening)
            {
                // Setup the connection listener
                MDSResponseBlock block = new MDSResponseBlock(async (arg0) => MdsConnectionListener.Current.OnListenerSetupCompleted(arg0));
                MDSEventBlock eventBlock = (MDSEvent arg0) => MdsConnectionListener.Current.OnDeviceConnectionEvent(arg0);
                ((MDSWrapper)(CrossMovesense.Current.MdsInstance)).DoSubscribe("MDS/ConnectedDevices", new Foundation.NSDictionary(), block, eventBlock);
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
                var serial = ((NSString)mdsevent.BodyDictionary.ValueForKey(new NSString("Serial")));
                // TODO extract Uuid from the BodyDictionary["DeviceInfo"] ??
                Debug.WriteLine($"MdsConnectionListener OnDeviceConnectionEvent CONNECTED: Serial {serial}");
                ConnectionComplete?.Invoke(this, new MdsConnectionListenerEventArgs(serial));
            }
            else if (method == new NSString("DEL"))
            {
                // Device disconnected
                var serial = ((NSString)mdsevent.BodyDictionary.ValueForKey(new NSString("Serial")));
                Debug.WriteLine($"MdsConnectionListener OnDeviceConnectionEvent DISCONNECTED: Serial {serial}");
                Disconnect?.Invoke(this, new MdsConnectionListenerEventArgs(serial));
            }
            else
            {
                throw new MdsException($"OnDeviceConnectionEvent unexpected method: {method}");
            }
        }
    }

    /// <summary>
    /// Event args for MdsConnectionListener events
    /// </summary>
    public class MdsConnectionListenerEventArgs : EventArgs
    {
        /// <summary>
        /// Serial number of the device
        /// </summary>
        public string Serial { get; set; }

        /// <summary>
        /// Uuid of the device
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// Creates MdsConnectionListenerEventArgs used for reporting connect events
        /// </summary>
        /// <param name="serial">Serial number of the device</param>
        /// <param name="uuid">Uuid of the device</param>
        public MdsConnectionListenerEventArgs(string serial, Guid uuid)
        {
            Serial = serial;
            Uuid = uuid;
        }
         /// <summary>
        /// Creates MdsConnectionListenerEventArgs used for reporting disconnect events
        /// </summary>
        /// <param name="serial">Serial number of the device</param>
        public MdsConnectionListenerEventArgs(string serial)
        {
            Serial = serial;
            Uuid = Guid.Empty;
        }
    }
}