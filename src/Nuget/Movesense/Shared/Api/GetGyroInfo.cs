using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GetGyroInfo : ApiCallAsync<GyroInfo>
    {
        private static readonly string GYRO_INFO_PATH = "/Meas/Gyro/Info";

        /// <summary>
        /// Get Gyrometer configuration, CallAsync returns GyroInfo
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public GetGyroInfo(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + GYRO_INFO_PATH, null, responseListener);
        }
    }
}
