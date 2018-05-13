using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GetIMUInfo : ApiCallAsync<IMUInfo>
    {
        private static string IMU_INFO_PATH = "/Meas/IMU/Info";

        public GetIMUInfo(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + IMU_INFO_PATH, null, responseListener);
        }
    }
}
