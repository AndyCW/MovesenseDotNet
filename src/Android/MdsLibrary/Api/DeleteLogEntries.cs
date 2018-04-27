using Com.Movesense.Mds;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class DeleteLogEntries : ApiCallAsync
    {
        private static string LOGGET_ENTRIES_PATH = "/Mem/Logbook/Entries";

        public DeleteLogEntries(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            // In contract, override the default timeout - can take over 30 seconds
            mds.Delete(Mdx.SCHEME_PREFIX + serial + LOGGET_ENTRIES_PATH, null, responseListener);
        }
    }
}
