using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetLogEntries : ApiCallAsync<LogEntriesResult>
    {
        private static readonly string LOGGET_ENTRIES_PATH = "/Mem/Logbook/Entries";

        /// <summary>
        /// Get details of Logbook entries for a device, CallAsync returns LogEntriesResult 
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public GetLogEntries(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Get(Mdx.SCHEME_PREFIX + serial + LOGGET_ENTRIES_PATH, null, responseListener);
        }
    }
}
