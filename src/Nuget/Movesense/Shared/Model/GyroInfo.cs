using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    public class GyroInfo
    {
        [JsonProperty("Content")]
        public Content accInfo;

        public class Content
        {
            [JsonProperty("SampleRates")]
            public int[] mSampleRates;

            [JsonProperty("Ranges")]
            public int[] mRanges;

            public int[] GetSampleRates()
            {
                return mSampleRates;
            }
            public int[] GetRanges()
            {
                return mRanges;
            }
        }
    }
}

