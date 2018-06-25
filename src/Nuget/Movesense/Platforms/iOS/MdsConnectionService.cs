using Movesense;
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
        private TaskCompletionSource<object> connectiontcs;
        private TaskCompletionSource<object> disconnectTcs;

        /// <summary>
        /// Connect a device to MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns></returns>
        public async Task<object> ConnectMdsAsync(string Uuid)
        {
            mUuid = Uuid;
            connectiontcs = new TaskCompletionSource<object>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            // Ensure the connection listener is setup
            await mListener.EnsureInitializedAsync();

            // Listen for connect/disconnect events
            // TODO how do we filter connect events to make sure they are for this device? Does DeviceInfo on connection have Uuid in it?
            mListener.ConnectionComplete += MListener_ConnectionComplete;

            // Start the device connection
            ((Movesense.MDSWrapper)(CrossMovesense.Current.MdsInstance)).ConnectPeripheralWithUUID(new Foundation.NSUuid(Uuid));

            return connectiontcs.Task;
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns></returns>
        public async Task<object> DisconnectMds(string Uuid)
        {
            mUuid = Uuid;
            disconnectTcs = new TaskCompletionSource<object>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            // Ensure the connection listener is setup
            await mListener.EnsureInitializedAsync();

            mListener.Disconnect += MListener_Disconnect;

            ((Movesense.MDSWrapper)(CrossMovesense.Current.MdsInstance)).DisconnectPeripheralWithUUID(new Foundation.NSUuid(Uuid));

            return disconnectTcs.Task;
        }

        private void MListener_ConnectionComplete(object sender, MdsConnectionListenerEventArgs e)
        {
            if (e.MACAddress == mUuid)
            {
                connectiontcs?.TrySetResult(null);
                mListener.ConnectionComplete -= MListener_ConnectionComplete;
            }
        }

        private void MListener_Disconnect(object sender, MdsConnectionListenerEventArgs e)
        {
            if (e.MACAddress == mUuid)
            {
                disconnectTcs?.TrySetResult(null);
                mListener.Disconnect -= MListener_Disconnect;
            }
        }
    }
}

