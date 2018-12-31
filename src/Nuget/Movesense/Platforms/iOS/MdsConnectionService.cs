#if __IOS__
using Plugin.Movesense;
using System.Threading.Tasks;

namespace MdsLibrary
{
    /// <summary>
    /// Connection logic for iOS devices
    /// </summary>

    public class MdsConnectionService
    {
        private MdsConnectionListener mListener;
        private string mUuid;
        private TaskCompletionSource<MdsConnectionContext> connectiontcs;
        private TaskCompletionSource<object> disconnectTcs;

        /// <summary>
        /// Connect a device to MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns></returns>
        public Task<MdsConnectionContext> ConnectMdsAsync(string Uuid)
        {
            mUuid = Uuid.ToUpper();
            connectiontcs = new TaskCompletionSource<MdsConnectionContext>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            // Ensure the connection listener is setup
            mListener.EnsureInitializedAsync().Wait();

            // Listen for connect/disconnect events
            mListener.DeviceConnectionComplete += MListener_ConnectionComplete;

            // Start the device connection
            ((Movesense.MDSWrapper)(CrossMovesense.Current.MdsInstance)).ConnectPeripheralWithUUID(new Foundation.NSUuid(Uuid));

            return connectiontcs.Task;
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns></returns>
        public Task<object> DisconnectMdsAsync(string Uuid)
        {
            mUuid = Uuid.ToUpper();
            disconnectTcs = new TaskCompletionSource<object>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            // Ensure the connection listener is setup
            mListener.EnsureInitializedAsync().Wait();

            mListener.DeviceDisconnected += MListener_Disconnect;

            ((Movesense.MDSWrapper)(CrossMovesense.Current.MdsInstance)).DisconnectPeripheralWithUUID(new Foundation.NSUuid(Uuid));

            return disconnectTcs.Task;
        }

        private void MListener_ConnectionComplete(object sender, MdsConnectionListenerEventArgs e)
        {
            var serial = string.Empty;
            MdsConnectionListener.Current.MACAddressToSerialMapper.TryGetValue(mUuid, out serial);
            if (e.Serial.ToUpper() == serial)
            {
                connectiontcs?.TrySetResult(new MdsConnectionContext(serial, mUuid));
                mListener.DeviceConnectionComplete -= MListener_ConnectionComplete;
            }
        }

        private void MListener_Disconnect(object sender, MdsConnectionListenerEventArgs e)
        {
            var serial = string.Empty;
            MdsConnectionListener.Current.MACAddressToSerialMapper.TryGetValue(mUuid, out serial);

            if (e.Serial.ToUpper() == serial)
            {
                disconnectTcs?.TrySetResult(null);
                mListener.DeviceDisconnected -= MListener_Disconnect;
            }
        }
    }
}
#endif