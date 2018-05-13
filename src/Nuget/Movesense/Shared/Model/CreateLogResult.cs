using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    public class CreateLogResult
    {
        [JsonProperty("Content")]
        public int mLogId;

        public int GetLogId()
        {
            return mLogId;
        }
    }
}
