using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class CreateLogEntry : ApiCallAsync<CreateLogResult>
    {
        private static string LOGGER_ENTRIES_PATH = "/Mem/Logbook/Entries";

        public CreateLogEntry(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        { }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Post(Mdx.SCHEME_PREFIX + serial + LOGGER_ENTRIES_PATH, null, responseListener);
        }
    }
}
