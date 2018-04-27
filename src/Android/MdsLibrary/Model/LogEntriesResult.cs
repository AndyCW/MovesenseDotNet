using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    public class LogEntriesResult
    {
        [JsonProperty("Content")]
        public Content mContent;

        public class Content
        {
            [JsonProperty("elements")]
            public LogEntry[] mElements;

            public LogEntry[] GetElements()
            {
                return mElements;
            }
        }


        public class LogEntry
        {

            [JsonProperty("Id")]
            public int mId;

            [JsonProperty("ModificationTimestamp")]
            public long mModificationTimestamp;

            public int GetId()
            {
                return mId;
            }

            public long GetModificationTimestamp()
            {
                return mModificationTimestamp;
            }
        }

        public Content GetContent()
        {
            return mContent;
        }
    }
}
