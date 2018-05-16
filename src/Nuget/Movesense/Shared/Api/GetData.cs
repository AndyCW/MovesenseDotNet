using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class GetData : ApiCallAsync<BaseResult>
    {
        private static readonly string LOGBOOK_DATA_PATH = "/Mem/Logbook/byId/{0}/Data";
        private int mLogId;

        /// <summary>
        /// Get data from a Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="logId">Number of the entry to get</param>
        public GetData(string deviceName, int logId) :
            base(deviceName)
        {
            mLogId = logId;
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            string datapath = String.Format(LOGBOOK_DATA_PATH, mLogId);
            mds.Get(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + datapath, null, responseListener);
        }
    }
}
