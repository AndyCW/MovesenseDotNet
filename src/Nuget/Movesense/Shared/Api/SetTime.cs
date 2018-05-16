using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Com.Movesense.Mds;

namespace MdsLibrary.Api
{
    public class SetTime : ApiCallAsync
    {
        private static readonly string TIME_PATH = "/Time";

        /// <summary>
        /// Set clock time on the device to the time on the phone (in ms)
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public SetTime(string deviceName) :
            base(deviceName)
        {
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            long timems = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            string time = $"{{\"value\":{timems * 1000}}}";
            Debug.WriteLine($"INFO SetTime TIME {time}");
            mds.Put(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + TIME_PATH, time, responseListener);
        }
    }
}
