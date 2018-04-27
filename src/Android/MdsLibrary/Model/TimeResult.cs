using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    public class TimeResult
    {
        [JsonProperty("Content")]
        public long mTime;

        public long GetTime()
        {
            return mTime;
        }
    }
}
