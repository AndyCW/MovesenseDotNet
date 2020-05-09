#if __ANDROID__
using Plugin.Movesense;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdsLibrary
{
    /// <summary>
    /// Connection logic for Android devices
    /// </summary>

    public partial class MdsConnectionService : IMdsConnectionService
    {
        private MdsConnectionListener mListener;
        private Guid mUuid;
        private string mMACAddress;
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
        /// <param name="Uuid">Unique ID of the device, includes MAC address</param>
        /// <returns></returns>
        public Task<IMovesenseDevice> ConnectMdsAsync(Guid Uuid)
        {
            mUuid = Uuid;
            mMACAddress = GetMACAddressFromUuid(Uuid);
            connectiontcs = new TaskCompletionSource<IMovesenseDevice>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            mListener.DeviceConnectionComplete += MListener_DeviceConnectionComplete;

            // Start the connection
            ((Com.Movesense.Mds.Mds)(CrossMovesense.Current.MdsInstance)).Connect(mMACAddress, mListener);

            return connectiontcs.Task;
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="Uuid">Unique ID of the device, includes MAC address</param>
        /// <returns></returns>
        public Task<object> DisconnectMdsAsync(Guid Uuid)
        {
            mUuid = Uuid;
            mMACAddress = GetMACAddressFromUuid(Uuid);
            disconnectTcs = new TaskCompletionSource<object>();
            // Get the single instance of the connection listener
            mListener = MdsConnectionListener.Current;
            mListener.DeviceDisconnected += MListener_DeviceDisconnected;

            ((Com.Movesense.Mds.Mds)(CrossMovesense.Current.MdsInstance)).Disconnect(mMACAddress);

            return disconnectTcs.Task;
        }

        private void MListener_DeviceDisconnected(object sender, MdsConnectionListenerEventArgs e)
        {
            var serial = string.Empty;
            MdsConnectionListener.Current.UuidToSerialMapper.TryGetValue(mUuid, out serial);
            if (e.Serial == serial)
            {
                disconnectTcs?.TrySetResult(null);
                mListener.DeviceDisconnected -= MListener_DeviceDisconnected;
            }
        }

        private void MListener_DeviceConnectionComplete(object sender, MdsConnectionListenerEventArgs e)
        {
            var serial = string.Empty;
            MdsConnectionListener.Current.UuidToSerialMapper.TryGetValue(mUuid, out serial);
            if (e.Serial == serial)
            {
                connectiontcs?.TrySetResult(new MdsMovesenseDevice(serial, mUuid));
                mListener.DeviceConnectionComplete -= MListener_DeviceConnectionComplete;
            }
        }

        private string GetMACAddressFromUuid(Guid Uuid)
        {
            string[] idParts = Uuid.ToString().Split(new char[] { '-' });
            string macAddress = idParts.Last().ToUpper();
            StringBuilder formattedMAC = new StringBuilder();
            for (int i = 0; i < macAddress.Length; i += 2)
            {
                if (i > 0) formattedMAC.Append(":");
                formattedMAC.Append(macAddress.Substring(i, 2));
            }

            return formattedMAC.ToString();
        }
    }
}
#endif