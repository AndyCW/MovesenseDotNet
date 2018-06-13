using Newtonsoft.Json;

namespace MdsLibrary.Model
{
    /// <summary>
    /// Periodic magnetometer measurements
    /// </summary>
    public class MagnData
    {
        /// <summary>
        /// Periodic magnetometer measurements
        /// </summary>
        [JsonProperty("Body")]
        public Body Data;

        /// <summary>
        /// Builds a MagnData
        /// </summary>
        /// <param name="body"></param>
        public MagnData(Body body)
        {
            this.Data = body;
        }

        /// <summary>
        /// Contains data for Periodic magnetometer measurements
        /// </summary>
        public class Body
        {
            /// <summary>
            /// Local timestamp of first measurement.
            /// </summary>
            [JsonProperty("Timestamp")]
            public long Timestamp;

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
        /// Measured magnetometer values (3D).
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
            /// Creates a Values3D
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
