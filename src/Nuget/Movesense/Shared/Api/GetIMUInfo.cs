using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GetIMUInfo : ApiCallAsync<IMUInfo>
    {
        private static readonly string IMU_INFO_PATH = "/Meas/IMU/Info";

        /// <summary>
        /// Get IMU configuration, CallAsync returns IMUInfo object
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public GetIMUInfo(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + IMU_INFO_PATH, null, responseListener);
        }
    }
}
