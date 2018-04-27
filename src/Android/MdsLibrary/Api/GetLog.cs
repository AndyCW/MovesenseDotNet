using Com.Movesense.Mds;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetLog : ApiCallAsync<string>
    {
        private static string URI_MDS_LOGBOOK_DATA = "suunto://MDS/Logbook/{0}/ById/{1}/Data";
        private int mLogBookId;

        public GetLog(bool? cancelled, string deviceName, int logBookId) :
            base(cancelled, deviceName)
        {
            mLogBookId = logBookId;
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            string path = String.Format(URI_MDS_LOGBOOK_DATA, serial, mLogBookId);
            mds.Get(path, null, responseListener);
        }
    }
}
