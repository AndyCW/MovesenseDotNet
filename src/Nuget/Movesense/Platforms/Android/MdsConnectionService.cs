using Plugin.Movesense;
using System.Threading.Tasks;

namespace MdsLibrary
{
    /// <summary>
    /// Connection logic for Android devices
    /// </summary>
    public class MdsConnectionService
    {
        private MdsConnectionListener mListener;
        private string mMACAddress;
        private TaskCompletionSource<object> connectiontcs;
        private TaskCompletionSource<object> disconnectTcs;

        /// <summary>
        /// Connect a device to MdsLib
        /// </summary>
        /// <param name="MACAddress">MAC address of the device</param>
        /// <returns></returns>
        public Task<object> ConnectMdsAsync(string MACAddress)
        {
            mMACAddress = MACAddress;
            connectiontcs = new TaskCompletionSource<object>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            mListener.DeviceConnectionComplete += MListener_DeviceConnectionComplete;

            // Start the connection
            ((Com.Movesense.Mds.Mds)(CrossMovesense.Current.MdsInstance)).Connect(MACAddress, mListener);

            return connectiontcs.Task;
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="MACAddress">MAC address of the device</param>
        /// <returns></returns>
        public Task<object> DisconnectMdsAsync(string MACAddress)
        {
            mMACAddress = MACAddress;
            disconnectTcs = new TaskCompletionSource<object>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            mListener.DeviceDisconnected += MListener_DeviceDisconnected;

            ((Com.Movesense.Mds.Mds)(CrossMovesense.Current.MdsInstance)).Disconnect(MACAddress);

            return disconnectTcs.Task;
        }

        private void MListener_DeviceDisconnected(object sender, MdsConnectionListenerEventArgs e)
        {
            if (e.MACAddress == mMACAddress)
            {
                disconnectTcs?.TrySetResult(null);
                mListener.DeviceDisconnected -= MListener_DeviceDisconnected;
            }
        }

        private void MListener_DeviceConnectionComplete(object sender, MdsConnectionListenerEventArgs e)
        {
            if (e.MACAddress == mMACAddress)
            {
                connectiontcs?.TrySetResult(null);
                mListener.DeviceConnectionComplete -= MListener_DeviceConnectionComplete;
            }
        }

    }
}

