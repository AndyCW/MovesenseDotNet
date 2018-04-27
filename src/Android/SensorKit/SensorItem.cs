using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SensorKitSDK
{
    [DataContract]
    public class SensorItem : ViewModel
    {
        DateTime _timestamp = DateTime.MinValue;
        [DataMember]
        public DateTime timestamp { get { return _timestamp; } set { SetValue(ref _timestamp, value, "timestamp"); } } 

        [IgnoreDataMember]
        public double offsetMs { get; set; } = 0.0;

        [DataMember]
        public SensorItemTypes itemType { get; set; }

        double _duration;
        [DataMember]
        public double duration { get { return _duration; } set { SetValue(ref _duration, value, "duration", "DurationTime"); } }

        double _force;
        [DataMember]
        public double force { get { return _force; } set { SetValue(ref _force, value, "force", "GForce"); } }

        double _distance;
        [DataMember]
        public double distance { get { return _distance; } set { SetValue(ref _distance, value, "distance"); } }

        double _altitude;
        [DataMember]
        public double altitude { get { return _altitude; } set { SetValue(ref _altitude, value, "altitude"); } }

        double _radius;
        [DataMember]
        public double radius { get { return _radius; } set { SetValue(ref _radius, value, "radius"); } }

        double _steps;
        [DataMember]
        public double steps { get { return _steps; } set { SetValue(ref _steps, value, "steps"); } }

        public SensorItem() { }

        [IgnoreDataMember]
        public TimeSpan DurationTime
        {
            get
            {
                return TimeSpan.FromMilliseconds(duration);
            }
        }

        [IgnoreDataMember]
        public double GForce
        {
            get
            {
                return force / 9.80665;
            }
        }

        public double ax { get; set; }
        public double ay { get; set; }
        public double az { get; set; }
        public double xl { get; set; }
        public double xh { get; set; }
        public double yl { get; set; }
        public double yh { get; set; }
        public double zl { get; set; }
        public double zh { get; set; }
    }
   
}
