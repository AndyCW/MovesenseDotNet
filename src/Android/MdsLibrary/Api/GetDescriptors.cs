using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetDescriptors : ApiCallAsync<BaseResult>
    {
        private static string URI_MDS_LOGBOOK_DATA = "/Mem/Logbook/byId/{0}/Descriptors";
        private int mLogId;

        public GetDescriptors(bool? cancelled, string deviceName, int logId) :
            base(cancelled, deviceName)
        {
            mLogId = logId;
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            string datapath = String.Format(URI_MDS_LOGBOOK_DATA, mLogId);
            mds.Get(Mdx.SCHEME_PREFIX + serial + datapath, null, responseListener);
        }
    }
}
