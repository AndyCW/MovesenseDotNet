using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    public class LogStatusResult 
    {
        [JsonProperty("Content")]
        public int mStatus;

        public int GetStatus()
        {
            return mStatus;
        }
    }
}
