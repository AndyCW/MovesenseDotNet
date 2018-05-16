using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetLeds : ApiCallAsync<LedsResult>
    {
        private static readonly string LED_PATH = "/Component/Leds";

        /// <summary>
        /// Get LedsResult object giving state of all Leds
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public GetLeds(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + LED_PATH, null, responseListener);
        }
    }
}
