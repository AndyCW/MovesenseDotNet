#if __IOS__
using Plugin.Movesense;
using System;
using System.Threading.Tasks;

namespace MdsLibrary
{
    /// <summary>
    /// Connection logic for iOS devices
    /// </summary>
    public partial class MdsConnectionService : IMdsConnectionService
    {
        private MdsConnectionListener mListener;
        private Guid mUuid;
        private TaskCompletionSource<IMovesenseDevice> connectiontcs;
        private TaskCompletionSource<object> disconnectTcs;


        /// <summary>
        ///  Return an instance of MdsConnection Service
        /// </summary>
        public static IMdsConnectionService GetInstance()
        {
            return new MdsConnectionService();
        }

        /// <summary>
        /// Connect a device to MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns></returns>
        public async Task<IMovesenseDevice> ConnectMdsAsync(Guid Uuid)
        {
            mUuid = Uuid;
            connectiontcs = new TaskCompletionSource<IMovesenseDevice>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            // Ensure the connection listener is setup
            await mListener.EnsureInitializedAsync().ConfigureAwait(false);

            // Listen for connect/disconnect events
            mListener.DeviceConnectionComplete += MListener_ConnectionComplete;

            // Start the device connection
            ((Movesense.MDSWrapper)(CrossMovesense.Current.MdsInstance)).ConnectPeripheralWithUUID(new Foundation.NSUuid(mUuid.ToString()));

            return await connectiontcs.Task.ConfigureAwait(false);
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns></returns>
        public async Task<object> DisconnectMdsAsync(Guid Uuid)
        {
            mUuid = Uuid;
            disconnectTcs = new TaskCompletionSource<object>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            // Ensure the connection listener is setup
            await mListener.EnsureInitializedAsync().ConfigureAwait(false);

            mListener.DeviceDisconnected += MListener_Disconnect;

            ((Movesense.MDSWrapper)(CrossMovesense.Current.MdsInstance)).DisconnectPeripheralWithUUID(new Foundation.NSUuid(mUuid.ToString()));

            return await disconnectTcs.Task.ConfigureAwait(false);
        }

        private void MListener_ConnectionComplete(object sender, MdsConnectionListenerEventArgs e)
        {
            var serial = string.Empty;
            MdsConnectionListener.Current.UuidToSerialMapper.TryGetValue(mUuid, out serial);
            if (e.Serial.ToUpper() == serial)
            {
                connectiontcs?.TrySetResult(new MdsMovesenseDevice(serial, mUuid));
                mListener.DeviceConnectionComplete -= MListener_ConnectionComplete;
            }
        }

        private void MListener_Disconnect(object sender, MdsConnectionListenerEventArgs e)
        {
            var serial = string.Empty;
            MdsConnectionListener.Current.UuidToSerialMapper.TryGetValue(mUuid, out serial);

            if (e.Serial.ToUpper() == serial)
            {
                disconnectTcs?.TrySetResult(null);
                mListener.DeviceDisconnected -= MListener_Disconnect;
            }
        }
    }
}
#endif