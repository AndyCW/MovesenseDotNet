using System;
using Com.Movesense.Mds;

namespace MdsLibrary.Api
{
    public class SetLed : ApiCallAsync
    {
        private bool mLedOn;
        private int mLedIndex;
        private LedColor mLedColor;
        private static string LED_PATH = "/Component/Leds/{0}";

        public SetLed(bool? cancelled, string deviceName, int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red) :
            base(cancelled, deviceName)
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
            mds.Put(Mdx.SCHEME_PREFIX + serial + datapath, mLedOn ? led_On_Body : led_Off_Body, responseListener);
        }
    }

    public enum LedColor
    {
        Red = 0,
        Green,
        Blue
    }
}
