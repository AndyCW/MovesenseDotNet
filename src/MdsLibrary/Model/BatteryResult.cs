using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    public class BatteryResult
    {
        [JsonProperty("Content")]
        public int mChargePercent;

        public int GetBatteryLevel()
        {
            return mChargePercent;
        }
    }
}
