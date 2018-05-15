using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetDescriptors : ApiCallAsync<BaseResult>
    {
        private static readonly string URI_MDS_LOGBOOK_DATA = "/Mem/Logbook/byId/{0}/Descriptors";
        private int mLogId;
        
        /// <summary>
        /// Get Deascriptors for a Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="logId">Logbook entry to get</param>
        public GetDescriptors(string deviceName, int logId) :
            base(deviceName)
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
