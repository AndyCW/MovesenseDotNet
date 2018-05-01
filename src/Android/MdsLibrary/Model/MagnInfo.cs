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
    public class MagnInfo
    {
        [JsonProperty("Content")]
        public Content accInfo;

        public class Content
        {
            [JsonProperty("SampleRates")]
            public int[] mSampleRates;

            [JsonProperty("Scale")]
            public int[] mScale;

            public int[] GetSampleRates()
            {
                return mSampleRates;
            }
            public int[] GetScale()
            {
                return mScale;
            }
        }
    }
}

