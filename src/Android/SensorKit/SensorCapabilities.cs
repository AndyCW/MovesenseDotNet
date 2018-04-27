using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorKitSDK
{
    public enum SensorCapabilities : int
    {
        Hub = 10000, // sensor hub device, e.g. mobile device

        WinterSports = 0,
        Ski = 1,
        Snowboard = 2,
        TelemarkSki = 3,
        CrossCountrySki = 4,
        Running = 1000,
        Cycling = 1001,
        MountainBike = 1002,
        CyclingStationary = 1003,
        Skateboard = 1004,
        Swimming = 1005,
        Scooter = 1006,
        Walking = 1007,
        InlineSkating = 1008,
        Soccer = 1009,
        Shopping = 1010,
        Hiking = 1011,
        DogWalking = 1012,
        Golf = 1013,
        MotorBiking = 1014,
        Dancing = 1015,
        SnowMobile = 1016,
        Threadmill = 1017,
        Stilts = 1018,
        Yoga = 1019,
        Tennis = 1020,
        Elliptical = 1021,
        PogoStick = 1022,
        Cheerleading = 1023,
        VideoLesson = 50001,
        ReadLesson = 50002,
        DayOff = 50003,
        Kayaking = 1024,
        Rowing = 1025,
        Sailing = 1026,
        Windsurfing = 1027,
        JetSki = 1028,
        Weights = 1029,
        CasualRunning = 1100,
        CasualWalking = 1101,
        HorsebackRiding = 1102,
        Trekking = 1103,
        NordicWalking = 1104,
        Wheelchair = 1105,
        Javelin = 1201,
        Kiteboarding = 1202,
        Surfing = 1203,
        Paddleboard = 1204,
        Rafting = 1205,
        Wakeboarding = 1206,
        Diving = 1207,
        Snorkeling = 1208,

        IMU,
        ChangeColor,
        Button,
        Mute,
        AirTime,
        Raw,
        Battery
    }
}
