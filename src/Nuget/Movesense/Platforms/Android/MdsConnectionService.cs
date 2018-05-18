using Plugin.Movesense;
using System.Threading.Tasks;

namespace MdsLibrary
{
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
            mListener.ConnectionComplete += MListener_ConnectionComplete;

            // Start the connection
            ((Com.Movesense.Mds.Mds)(CrossMovesense.Current.MdsInstance)).Connect(MACAddress, mListener);

            return connectiontcs.Task;
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="MACAddress">MAC address of the device</param>
        /// <returns></returns>
        public Task<object> DisconnectMds(string MACAddress)
        {
            mMACAddress = MACAddress;
            disconnectTcs = new TaskCompletionSource<object>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            mListener.Disconnect += MListener_Disconnect;

            ((Com.Movesense.Mds.Mds)(CrossMovesense.Current.MdsInstance)).Disconnect(MACAddress);

            return disconnectTcs.Task;
        }

        private void MListener_Disconnect(object sender, MdsConnectionListenerEventArgs e)
        {
            if (e.MACAddress == mMACAddress)
            {
                disconnectTcs?.TrySetResult(null);
                mListener.Disconnect -= MListener_Disconnect;
            }
        }

        private void MListener_ConnectionComplete(object sender, MdsConnectionListenerEventArgs e)
        {
            if (e.MACAddress == mMACAddress)
            {
                connectiontcs?.TrySetResult(null);
                mListener.ConnectionComplete -= MListener_ConnectionComplete;
            }
        }

    }
}

