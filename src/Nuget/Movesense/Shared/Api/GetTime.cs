using System;
using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GetTime : ApiCallAsync<TimeResult>
    {
        private static string TIME_PATH = "/Time";

        public GetTime(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + TIME_PATH, null, responseListener);
        }
    }
}
