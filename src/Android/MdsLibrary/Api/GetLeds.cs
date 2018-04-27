using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetLeds : ApiCallAsync<LedsResult>
    {
        private static string LED_PATH = "/Component/Leds";

        public GetLeds(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + LED_PATH, null, responseListener);
        }
    }
}
