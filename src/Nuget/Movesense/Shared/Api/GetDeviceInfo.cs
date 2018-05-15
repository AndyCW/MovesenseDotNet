using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GetDeviceInfo : ApiCallAsync<DeviceInfoResult>
    {
        private static readonly string INFO_PATH = "/Info";

        /// <summary>
        /// Get device info, CallAsync returns DeviceInfoResult
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public GetDeviceInfo(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + INFO_PATH, null, responseListener);
        }
    }
}
