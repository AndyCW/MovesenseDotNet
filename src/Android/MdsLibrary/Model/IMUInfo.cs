using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    public class IMUInfo
    {
        [JsonProperty("Content")]
        public Content accInfo;

        public class Content
        {
            [JsonProperty("SampleRates")]
            public int[] mSampleRates;

            [JsonProperty("AccRanges")]
            public int[] mAccRanges;

            [JsonProperty("GyroRanges")]
            public int[] mGyroRanges;

            [JsonProperty("MagnRanges")]
            public int[] mMagnRanges;

            public int[] GetSampleRates()
            {
                return mSampleRates;
            }

            public int[] GetAccRanges()
            {
                return mAccRanges;
            }

            public int[] GetMagnRanges()
            {
                return mMagnRanges;
            }

            public int[] GetGyroRanges()
            {
                return mGyroRanges;
            }
        }
    }
}
