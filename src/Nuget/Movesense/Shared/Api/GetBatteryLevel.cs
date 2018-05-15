using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetBatteryLevel : ApiCallAsync<BatteryResult>
    {
        private static readonly string BATTERY_PATH = "/System/Energy/Level";

        /// <summary>
        /// Get Battery level, CallAsync returns BatteryResult
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public GetBatteryLevel(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + BATTERY_PATH, null, responseListener);
        }
    }
}
