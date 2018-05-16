using System;
using Com.Movesense.Mds;

namespace MdsLibrary.Api
{
    public class SetLed : ApiCallAsync
    {
        private readonly bool mLedOn;
        private readonly int mLedIndex;
        private readonly LedColor mLedColor;
        private static string LED_PATH = "/Component/Leds/{0}";

        /// <summary>
        /// Sets state of an LED
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="ledIndex">Index of the Led - use 0 for standard Movesense sensor</param>
        /// <param name="ledOn">Set on or off</param>
        /// <param name="ledColor">[optional]value from LedColor enumeration - default is LedColor.Red</param>


        public SetLed(string deviceName, int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red) :
            base(deviceName)
        {
            mLedIndex = ledIndex;
            mLedOn = ledOn;
            mLedColor = ledColor;
        }

        protected override void performCall(Mds mds, string serial, IMdsResponseListener responseListener)
        {
            string datapath = String.Format(LED_PATH, mLedIndex);
            string led_On_Body = $"{{ \"LedState\": {{ \"IsOn\": true, \"LedColor\": {(int)mLedColor}}} }}";
            string led_Off_Body = @"{ ""LedState"": { ""IsOn"": false, ""LedColor"": 0} }";
            mds.Put(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + datapath, mLedOn ? led_On_Body : led_Off_Body, responseListener);
        }
    }

    public enum LedColor
    {
        Red = 0,
        Green,
        Blue
    }
}
