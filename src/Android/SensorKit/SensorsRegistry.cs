using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorKitSDK
{
    public class SensorsRegistry
    {
        List<SensorInformation> _registry = new List<SensorInformation>()
        {
            new SensorInformation{ Model=SensorTypes.Hub, ShortDescription = "Sensor Kit", Capabilities = new List<SensorCapabilities>{SensorCapabilities.Hub } },
            new SensorInformation{ Model=SensorTypes.Zmove, Manufacturer="Zmove", Url = "https://zmove.io/products/zmove", ShortDescription = "Universal sports sensor",  Capabilities = new List<SensorCapabilities> {SensorCapabilities.IMU, SensorCapabilities.AirTime, SensorCapabilities.Battery, SensorCapabilities.Ski, SensorCapabilities.TelemarkSki, SensorCapabilities.Snowboard, SensorCapabilities.Kiteboarding, SensorCapabilities.Surfing, SensorCapabilities.MountainBike, SensorCapabilities.Skateboard, SensorCapabilities.Windsurfing}, Format="Zmove"},
            //new SensorInformation{ Model=SensorTypes.illumiSens, Manufacturer="Zmove", Url="https://zmove.io/products/illumisens-body-sensors", ShortDescription = "Sensors with straps", Capabilities = new List<SensorCapabilities> {SensorCapabilities.IMU, SensorCapabilities.Ski, SensorCapabilities.TelemarkSki, SensorCapabilities.Snowboard }},
            //new SensorInformation{ Model=SensorTypes.illumiSki, Manufacturer="Zmove", Url = "https://zmove.io/products/illumiski-ski-sensor", ShortDescription = "Illuminated fun sensor", Capabilities = new List<SensorCapabilities> {SensorCapabilities.IMU, SensorCapabilities.ChangeColor, SensorCapabilities.AirTime, SensorCapabilities.Ski, SensorCapabilities.TelemarkSki, SensorCapabilities.Snowboard }},
            //new SensorInformation{ Model=SensorTypes.illumiBand, Manufacturer="Zmove", Url="https://zmove.io/products/illumiband-wearable-sensors", ShortDescription = "Wearable band", Capabilities = new List<SensorCapabilities> { SensorCapabilities.IMU, SensorCapabilities.Button, SensorCapabilities.AirTime, SensorCapabilities.Ski, SensorCapabilities.TelemarkSki, SensorCapabilities.Snowboard }},
            //new SensorInformation{ Model=SensorTypes.Javelin, Manufacturer="Zmove", Url="http://cyrushostetler.com/", ShortDescription = "Javelin by Olympian Cyrus Hostetler", Capabilities = new List<SensorCapabilities> {SensorCapabilities.IMU, SensorCapabilities.Javelin}},
             new SensorInformation{ Model=SensorTypes.MoveSense, Manufacturer="MoveSense", Url="http://movesense.com/", ShortDescription = "MoveSense", Capabilities = new List<SensorCapabilities> {SensorCapabilities.IMU, SensorCapabilities.Ski}, Format = "Movesense"}
            //new SensorInformation{ Model=SensorTypes.Boosters, Manufacturer="Boosters", Url="https://boosterstrap.com/", ShortDescription = "Boosters Sensor", Capabilities = new List<SensorCapabilities> {SensorCapabilities.IMU | SensorCapabilities.AirTime }},
           // add your sensors...
        };

        public IEnumerable<SensorInformation> Public
        {
            get
            {
                return _registry.Where(s=>s.Model != SensorTypes.Hub);
            }
        }

        public IEnumerable<SensorInformation> Items
        {
            get
            {
                return _registry;
            }
        }

        public SensorsRegistry() { }

    }

    
}
