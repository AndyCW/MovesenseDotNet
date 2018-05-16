using System;
using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GetTime : ApiCallAsync<TimeResult>
    {
        private static readonly string TIME_PATH = "/Time";

        /// <summary>
        /// Get the Time set on the device, CallAsync retruns TimeResult
        /// </summary>
        /// <param name="deviceName"></param>
        public GetTime(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + TIME_PATH, null, responseListener);
        }
    }
}
