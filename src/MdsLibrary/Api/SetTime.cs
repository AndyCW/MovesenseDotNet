using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Com.Movesense.Mds;

namespace MdsLibrary.Api
{
    public class SetTime : ApiCallAsync
    {
        private static string TIME_PATH = "/Time";

        public SetTime(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            long timems = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            string time = $"{{\"value\":{timems * 1000}}}";
            Debug.WriteLine($"INFO SetTime TIME {time}");
            mds.Put(Mdx.SCHEME_PREFIX + serial + TIME_PATH, time, responseListener);
        }
    }
}
