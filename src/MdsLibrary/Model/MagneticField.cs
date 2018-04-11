using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Model
{
    public class MagneticField
    {
        [JsonProperty("Body")]
        public MagneticField.Body body;

        public MagneticField(MagneticField.Body body)
        {
            this.body = body;
        }

        public class Body
        {
            [JsonProperty("Timestamp")]
            public long timestamp;

            [JsonProperty("ArrayMagn")]
            public MagneticField.Array[] array;

            [JsonProperty("Headers")]
            public MagneticField.Headers header;

            public Body(long timestamp, MagneticField.Array[] array, MagneticField.Headers header)
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
