using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GetDeviceInfo : ApiCallAsync<DeviceInfoResult>
    {
        private static string INFO_PATH = "/Info";

        public GetDeviceInfo(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + INFO_PATH, null, responseListener);
        }
    }
}
