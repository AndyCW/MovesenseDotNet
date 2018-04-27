using System.Threading.Tasks;

namespace MdsLibrary
{
    public class MdsConnectionService
    {
        private MdsConnectionListener mListener;
        private string mMACAddress;
        private TaskCompletionSource<object> connectiontcs;
        private TaskCompletionSource<object> disconnectTcs;

        public Task<object> ConnectMdsAsync(string MACAddress)
        {
            mMACAddress = MACAddress;
            connectiontcs = new TaskCompletionSource<object>();
            // Get the single instance of the connection listener
            mListener = Mdx.MdsConnectionListener;
            mListener.ConnectionComplete += MListener_ConnectionComplete;

            // Start the connection
            Mdx.MdsInstance.Connect(MACAddress, mListener);

            return connectiontcs.Task;
        }

        public Task<object> DisconnectMds(string MACAddress)
        {
            mMACAddress = MACAddress;
            disconnectTcs = new TaskCompletionSource<object>();
            // Get the single instance of the connection listener
            mListener = Mdx.MdsConnectionListener;
            mListener.Disconnect += MListener_Disconnect;

            Mdx.MdsInstance.Disconnect(MACAddress);

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

