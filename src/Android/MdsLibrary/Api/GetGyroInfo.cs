using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GetGyroInfo : ApiCallAsync<GyroInfo>
    {
        private static string GYRO_INFO_PATH = "/Meas/Gyro/Info";

        public GetGyroInfo(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + GYRO_INFO_PATH, null, responseListener);
        }
    }
}
