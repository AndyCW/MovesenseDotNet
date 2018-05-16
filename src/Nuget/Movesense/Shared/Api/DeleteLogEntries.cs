using Com.Movesense.Mds;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class DeleteLogEntries : ApiCallAsync
    {
        private static readonly string LOGGET_ENTRIES_PATH = "/Mem/Logbook/Entries";

        /// <summary>
        /// Delete all the Logbook entries
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public DeleteLogEntries(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            // In contract, override the default timeout - can take over 30 seconds
            mds.Delete(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + LOGGET_ENTRIES_PATH, null, responseListener);
        }
    }
}
