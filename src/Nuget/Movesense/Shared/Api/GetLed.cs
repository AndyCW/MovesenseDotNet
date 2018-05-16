using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetLed : ApiCallAsync<LedState>
    {
        private readonly int mLedIndex;
        private static readonly string LED_PATH = "/Component/Leds/{0}";

        /// <summary>
        /// Get LedState for an LED
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="ledIndex">Number of the Led</param>
        public GetLed(string deviceName, int ledIndex) :
            base(deviceName)
        {
            mLedIndex = ledIndex;
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            string datapath = String.Format(LED_PATH, mLedIndex);
            mds.Get(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + datapath, null, responseListener);
        }
    }
}
