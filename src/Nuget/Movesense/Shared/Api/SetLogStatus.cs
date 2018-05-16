using System;
using Com.Movesense.Mds;

namespace MdsLibrary.Api
{
    public class SetLogStatus : ApiCallAsync
    {
        private readonly bool mStart;
        private static string DATALOGGER_STATE_PATH = "/Mem/DataLogger/State/";
        private static readonly string LOG_ON = "{\"newState\":3}";
        private static readonly string LOG_OFF = "{\"newState\":2}";

        /// <summary>
        /// Set state of the Datalogger
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="start">Set true to start the datalogger, false to stop</param>
        public SetLogStatus(string deviceName, bool start) :
            base(deviceName)
        {
            mStart = start;
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Put(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + DATALOGGER_STATE_PATH, mStart ? LOG_ON : LOG_OFF, responseListener);
        }
    }
}
