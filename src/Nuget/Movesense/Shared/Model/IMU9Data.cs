using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    public class IMU9Data
    {
        [JsonProperty("Body")]
        public Body body;

        public IMU9Data(Body body)
        {
            this.body = body;
        }

        public class Body
        {
            [JsonProperty("Timestamp")]
            public long timestamp;

            [JsonProperty("ArrayAcc")]
            public Array[] arrayAcc;

            [JsonProperty("ArrayGyro")]
            public Array[] arrayGyro;

            [JsonProperty("ArrayMagn")]
            public Array[] arrayMagn;

            [JsonProperty("Headers")]
            public Headers header;
        }

        public class Array
        {
            [JsonProperty("x")]
            public double x;

            [JsonProperty("y")]
            public double y;

            [JsonProperty("z")]
            public double z;

            public Array(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        public class Headers
        {
            [JsonProperty("Param0")]
            public int param0;

            public Headers(int param0)
            {
                this.param0 = param0;
            }
        }
    }

}
