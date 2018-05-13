using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetLogStatus : ApiCallAsync<LogStatusResult>
    {
        private static string DATALOGGER_STATE_PATH = "/Mem/DataLogger/State/";

        public GetLogStatus(bool? cancelled, string deviceName) : 
            base(cancelled, deviceName)
        { }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + DATALOGGER_STATE_PATH, null, responseListener);
        }
    }
}
