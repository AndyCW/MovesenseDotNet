using System;
using Com.Movesense.Mds;
using Newtonsoft.Json;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class SetupLog : ApiCallAsync
    {
        private static int DEFAULT_FREQ = 52;
        private int mFreq;
        private static string DATALOGGER_CONFIG_PATH = "/Mem/DataLogger/Config/";

        public SetupLog(bool? cancelled, string deviceName, int freq) :
            base(cancelled, deviceName)
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
/*                new DataLoggerConfig.DataEntry("/Meas/Acc/" + mFreq),
                new DataLoggerConfig.DataEntry("/Meas/Magn/" + mFreq),
                new DataLoggerConfig.DataEntry("/Meas/Gyro/" + mFreq),*/
                // Andy 27/11/17 Replace with IMU9 i/f
                new DataLoggerConfig.DataEntry("/Meas/IMU9/" + mFreq)
            };

            DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
            string jsonConfig = JsonConvert.SerializeObject(config);

            mds.Put(Mdx.SCHEME_PREFIX + serial + DATALOGGER_CONFIG_PATH, jsonConfig, responseListener);
        }
    }
}
