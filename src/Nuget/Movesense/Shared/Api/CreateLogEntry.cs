﻿using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class CreateLogEntry : ApiCallAsync<CreateLogResult>
    {
        private static readonly string LOGGER_ENTRIES_PATH = "/Mem/Logbook/Entries";

        /// <summary>
        /// Create a new Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public CreateLogEntry(string deviceName) :
            base(deviceName)
        { }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            mds.Post(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + LOGGER_ENTRIES_PATH, null, responseListener);
        }
    }
}
