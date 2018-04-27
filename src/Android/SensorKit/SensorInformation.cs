using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorKitSDK
{

    public class SensorInformation : ViewModel
    {
        public SensorTypes Model { get; set; }
        public string Manufacturer { get; set; }
        public string Url { get; set; }
        public string ShortDescription { get; set; }
        public List<SensorCapabilities> Capabilities { get; set; }
        public string Format { get; set; } = "SensorKit";

        public static SensorTypes TryParseSensorType(string name)
        {
            if (!String.IsNullOrEmpty(name)) {
                var names = Enum.GetNames(typeof(SensorTypes));
                var values = (SensorTypes[])Enum.GetValues(typeof(SensorTypes));
                for (int i=0; i<names.Length;i++)
                {
                    if (name.ToLower().Contains(names[i].ToLower()))
                    {
                        return values[i];
                    }
                }
            }
            return SensorTypes.Unknown;
        }

    }
}
