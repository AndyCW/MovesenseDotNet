using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetLed : ApiCallAsync<LedState>
    {
        private int mLedIndex;
        private static string LED_PATH = "/Component/Leds/{0}";

        public GetLed(bool? cancelled, string deviceName, int ledIndex) :
            base(cancelled, deviceName)
        {
            mLedIndex = ledIndex;
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            string datapath = String.Format(LED_PATH, mLedIndex);
            mds.Get(Mdx.SCHEME_PREFIX + serial + datapath, null, responseListener);
        }
    }
}
