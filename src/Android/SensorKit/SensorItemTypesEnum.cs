using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorKitSDK
{
    public enum SensorItemTypes : ushort
    {
        Null = 0,
        Airtime = 1, // duration, altitude, G force
        Diving = 2,
        Javelin = 3,
        Steps = 4,
        Raw = 5, // raw accelerometer and gyro data
        Turns = 6
    }
}
