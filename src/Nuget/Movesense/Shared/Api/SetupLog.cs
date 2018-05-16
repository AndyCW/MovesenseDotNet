using System;
using Com.Movesense.Mds;
using Newtonsoft.Json;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class SetupLog : ApiCallAsync
    {
        private static int DEFAULT_FREQ = 52;
        private readonly int mFreq;
        private static readonly string DATALOGGER_CONFIG_PATH = "/Mem/DataLogger/Config/";

        /// <summary>
        /// Set configuration for the Datalogger - ONLY sets IMU9
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="freq">Sampling rate, e.g. 26 for 26Hz</param>
        public SetupLog(string deviceName, int freq) :
            base(deviceName)
        {
            mFreq = freq;
            if (mFreq == 0)
            {
                mFreq = DEFAULT_FREQ;
            }
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            DataLoggerConfig.DataEntry[] entries = {
                new DataLoggerConfig.DataEntry("/Meas/IMU9/" + mFreq)
            };

            DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
            string jsonConfig = JsonConvert.SerializeObject(config);

            mds.Put(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + DATALOGGER_CONFIG_PATH, jsonConfig, responseListener);
        }
    }
}
