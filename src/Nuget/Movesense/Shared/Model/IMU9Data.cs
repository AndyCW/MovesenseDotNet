using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Response object for subscription to periodic 9-axis IMU measurements.
    /// </summary>
    public class IMU9Data
    {
        /// <summary>
        /// Response object for subscription to periodic 9-axis IMU measurements.
        /// </summary>
        [JsonProperty("Body")]
        public Data body;

        /// <summary>
        /// Creates an IMU9Data object
        /// </summary>
        /// <param name="body"></param>
        public IMU9Data(Data body)
        {
            this.body = body;
        }

        /// <summary>
        /// Container for IMU9 data readings
        /// </summary>
        public class Data
        {
            /// <summary>
            /// Local timestamp of first measurement.
            /// </summary>
            [JsonProperty("Timestamp")]
            public long Timestamp;

            /// <summary>
            /// Measured acceleration values (3D) in array.
            /// </summary>
            [JsonProperty("ArrayAcc")]
            public Values3D[] ArrayAcc;

            /// <summary>
            /// Measured angular velocity values (3D) in array.
            /// </summary>
            [JsonProperty("ArrayGyro")]
            public Values3D[] ArrayGyro;

            /// <summary>
            /// Measured magnetic field values (3D) in array.
            /// </summary>
            [JsonProperty("ArrayMagn")]
            public Values3D[] ArrayMagn;

            /// <summary>
            /// Headers
            /// </summary>
            [JsonProperty("Headers")]
            public Headers Header;
        }

        /// <summary>
        /// 3D values
        /// </summary>
        public class Values3D
        {
            /// <summary>
            /// X reading
            /// </summary>
            [JsonProperty("x")]
            public double X;

            /// <summary>
            /// Y reading
            /// </summary>
            [JsonProperty("y")]
            public double Y;

            /// <summary>
            /// Z reading
            /// </summary>
            [JsonProperty("z")]
            public double Z;

            /// <summary>
            /// Constructs Values3D
            /// </summary>
            /// <param name="x">X value</param>
            /// <param name="y">Y value</param>
            /// <param name="z">Z value</param>
            public Values3D(double x, double y, double z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }
        }

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
