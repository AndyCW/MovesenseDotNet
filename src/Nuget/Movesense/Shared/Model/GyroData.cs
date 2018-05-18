using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Response object containing gyrometer data readings
    /// </summary>
    public class GyroData
    {
        /// <summary>
        /// Response object containing gyrometer data readings
        /// </summary>
        [JsonProperty("Body")]
        public Body Data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body">Body object containing gryometer data readings</param>
        public GyroData(Body body)
        {
            this.Data = body;
        }

        /// <summary>
        /// Gyrometer data readings
        /// </summary>
        public class Body
        {
            /// <summary>
            /// Local timestamp of first measurement.
            /// </summary>
            [JsonProperty("Timestamp")]
            public long Timestamp;

            /// <summary>
            /// Measured angular velocity values (3D) in array.
            /// </summary>
            [JsonProperty("ArrayGyro")]
            public Values3D[] ArrayGyro;

            /// <summary>
            /// Headers
            /// </summary>
            [JsonProperty("Headers")]
            public Headers Header;

            /// <summary>
            /// Creates a Body that contains Gyrometer readings
            /// </summary>
            /// <param name="timestamp">Local timestamp of first measurement</param>
            /// <param name="array">Measured angular velocity values (3D) in array</param>
            /// <param name="header">Header</param>
            public Body(long timestamp, Values3D[] array, Headers header)
            {
                this.Timestamp = timestamp;
                this.ArrayGyro = array;
                this.Header = header;
            }
        }

        /// <summary>
        /// Measured angular velocity values (3D).
        /// </summary>
        public class Values3D
        {
            /// <summary>
            /// X value
            /// </summary>
            [JsonProperty("x")]
            public double X;

            /// <summary>
            /// Y value
            /// </summary>
            [JsonProperty("y")]
            public double Y;

            /// <summary>
            /// Z value
            /// </summary>
            [JsonProperty("z")]
            public double Z;

            /// <summary>
            /// Creates a GyroArray
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            public Values3D(double x, double y, double z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }
        }

        /// <summary>
        /// Headers
        /// </summary>
        public class Headers
        {
            [JsonProperty("Param0")]
            public int Param0;

            public Headers(int param0)
            {
                this.Param0 = param0;
            }
        }
    }

}
