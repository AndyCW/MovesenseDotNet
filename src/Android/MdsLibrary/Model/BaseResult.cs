using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    public class BaseResult
    {
        [JsonProperty("Content")]
        public string mContent;

        public string GetContent()
        {
            return mContent;
        }
    }
}
