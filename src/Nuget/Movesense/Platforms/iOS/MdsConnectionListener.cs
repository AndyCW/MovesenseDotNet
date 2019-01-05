#if __IOS__
using Foundation;
using Movesense;
using Plugin.Movesense;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MdsLibrary
{
    /// <summary>
    /// Listener for callbacks from MdsLib for device connections and disconnections
    /// </summary>

    public sealed partial class MdsConnectionListener
    {

        private static bool mIsListening = false;

        private TaskCompletionSource<bool> setuplistenertcs;

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

                var uniqueIDGuid = new Guid(uuid);
                this.UuidToSerialMapper.TryAdd(uniqueIDGuid, serial);

                Debug.WriteLine($"MdsConnectionListener OnDeviceConnectionEvent CONNECTED: Serial {serial}");
                DeviceConnected?.Invoke(this, new MdsConnectionListenerBLEConnectedEventArgs(uuid));
                DeviceConnectionComplete?.Invoke(this, new MdsConnectionListenerEventArgs(serial, uniqueIDGuid));
            }
            else if (method == new NSString("DEL"))
            {
                // Device disconnected
                var bodyDict = (NSDictionary)mdsevent.BodyDictionary.ValueForKey(new NSString("Body"));
                var serial = ((NSString)bodyDict.ValueForKey(new NSString("Serial"))).ToString();
                var connDict = (NSDictionary)bodyDict.ValueForKey(new NSString("Connection"));
                var uuid = ((NSString)connDict.ValueForKey(new NSString("UUID"))).ToString();

                var uniqueIDGuid = new Guid(uuid);

                Debug.WriteLine($"MdsConnectionListener OnDeviceConnectionEvent DISCONNECTED: Serial {serial}");
                DeviceDisconnected?.Invoke(this, new MdsConnectionListenerEventArgs(serial, uniqueIDGuid));
            }
            else
            {
                throw new MdsException($"OnDeviceConnectionEvent unexpected method: {method}");
            }
        }
    }
}
#endif