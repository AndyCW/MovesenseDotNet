using System;
using Com.Movesense.Mds;

namespace MdsLibrary.Api
{
    public class SetLogStatus : ApiCallAsync
    {
        private bool mStart;
        private static string DATALOGGER_STATE_PATH = "/Mem/DataLogger/State/";
        private static string LOG_ON = "{\"newState\":3}";
        private static string LOG_OFF = "{\"newState\":2}";

        public SetLogStatus(bool? cancelled, string deviceName, bool start) :
            base(cancelled, deviceName)
        {
            mStart = start;
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Put(Mdx.SCHEME_PREFIX + serial + DATALOGGER_STATE_PATH, mStart ? LOG_ON : LOG_OFF, responseListener);
        }
    }
}
