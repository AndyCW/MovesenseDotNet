using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Response for Get leds in the system and their state (on/off and possible color).
    /// </summary>
    public class LedsResult
    {
        /// <summary>
        /// Response for Get leds in the system and their state (on/off and possible color).
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public Content Content { get; set; }
    }

    /// <summary>
    /// Response for Get leds in the system and their state (on/off and possible color).
    /// </summary>
    public class Content
    {
        /// <summary>
        /// Array of LedState objects describing state of the Leds in the system
        /// </summary>
        [JsonProperty(PropertyName = "ledStates")]
        public List<LedState> LedStates { get; set; }
    }

    /// <summary>
    /// State of an Led
    /// </summary>
    public class LedState
    {
        /// <summary>
        /// Led is on when true, otherwise off
        /// </summary>
        [JsonProperty(PropertyName = "isOn")]
        public bool IsOn { get; set; }

        /// <summary>
        /// Color of the Led: 'Red' : 0, 'Green' : 1, 'Blue' : 2
        /// </summary>
        [JsonProperty(PropertyName = "ledColor")]
        public int LedColor { get; set; }
    }
}
