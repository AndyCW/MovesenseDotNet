using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetLogEntries : ApiCallAsync<LogEntriesResult>
    {
        private static string LOGGET_ENTRIES_PATH = "/Mem/Logbook/Entries";

        public GetLogEntries(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + LOGGET_ENTRIES_PATH, null, responseListener);
        }
    }
}
