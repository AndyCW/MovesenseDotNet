using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    public class AngularVelocity
    {

        [JsonProperty("Body")]
        public AngularVelocity.Body body;

        public AngularVelocity(AngularVelocity.Body body)
        {
            this.body = body;
        }

        public class Body
        {
            [JsonProperty("Timestamp")]
            public long timestamp;

            [JsonProperty("ArrayGyro")]
            public AngularVelocity.Array[] array;

            [JsonProperty("Headers")]
            public AngularVelocity.Headers header;

            public Body(long timestamp, AngularVelocity.Array[] array, AngularVelocity.Headers header)
            {
                this.timestamp = timestamp;
                this.array = array;
                this.header = header;
            }
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
