using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetBatteryLevel : ApiCallAsync<BatteryResult>
    {
        private static string BATTERY_PATH = "/System/Energy/Level";

        public GetBatteryLevel(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + BATTERY_PATH, null, responseListener);
        }
    }
}
