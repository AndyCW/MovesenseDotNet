using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetData : ApiCallAsync<BaseResult>
    {
        private static string LOGBOOK_DATA_PATH = "/Mem/Logbook/byId/{0}/Data";
        private int mLogId;

        public GetData(bool? cancelled, string deviceName, int logId) :
            base(cancelled, deviceName)
        {
            mLogId = logId;
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            string datapath = String.Format(LOGBOOK_DATA_PATH, mLogId);
            mds.Get(Mdx.SCHEME_PREFIX + serial + datapath, null, responseListener);
        }
    }
}
