using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GetAccInfo : ApiCallAsync<AccInfo>
    {
        private static readonly string ACC_INFO_PATH = "/Meas/Acc/Info";

        /// <summary>
        /// Get Accelerometer configuration, call returns an AccInfo object
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public GetAccInfo(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + ACC_INFO_PATH, null, responseListener);
        }
    }
}
