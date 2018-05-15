using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GetMagnInfo : ApiCallAsync<MagnInfo>
    {
        private static readonly string ACC_INFO_PATH = "/Meas/Magn/Info";

        /// <summary>
        /// Get Magnetomoter configuration data, CallAsync returns MagnInfo object
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        public GetMagnInfo(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + ACC_INFO_PATH, null, responseListener);
        }
    }
}
