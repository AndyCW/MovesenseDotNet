using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    // {"Content": {"LedStates": [{"IsOn": false, "LedColor": 0}]}}
    public class LedsResult
    {
        [JsonProperty(PropertyName = "content")]
        public Content Content { get; set; }
    }

    public class Content
    {
        [JsonProperty(PropertyName = "ledStates")]
        public List<LedState> LedStates { get; set; }
    }

    public class LedState
    {
        [JsonProperty(PropertyName = "isOn")]
        public bool IsOn { get; set; }

        [JsonProperty(PropertyName = "ledColor")]
        public int LedColor { get; set; }
    }
}
