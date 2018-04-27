using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorKitSDK
{

    public class SensorData
    {
        public SensorAirData airdata { get; set; }
        public SensorTurnData turndata { get; set; }
        public SensorRawData rawdata { get; set; }
        public SensorSummaryData summary { get; set; }
    }

    //public class SensorSummaryData : ViewModel
    //{
    //    public long t { get; set; }

    //    int _steps = 0;
    //    public int steps { get { return _steps; } set { SetValue(ref _steps, value, "steps"); } }

    //    int _air = 0;
    //    public int air { get { return _air; } set { SetValue(ref _air, value, "air"); } }

    //    double _airgmax = 0.0;
    //    public double airgmax { get { return _airgmax; } set { SetValue(ref _airgmax, value, "airgmax"); } }

    //    double _airaltmax = 0.0;
    //    public double airaltmax { get { return _airaltmax; } set { SetValue(ref _airaltmax, value, "airaltmax"); } }

    //    int _turns = 0;
    //    public int turns { get { return _turns; } set { SetValue(ref _turns, value, "turns"); } }

    //    double _tgmax = 0.0;
    //    public double tgmax { get { return _tgmax; } set { SetValue(ref _tgmax, value, "tgmax"); } }

    //    double _tgavg = 0.0;
    //    public double tgavg { get { return _tgavg; } set { SetValue(ref _tgavg, value, "tgavg"); } }

    //    double _travg = 0.0;
    //    public double travg { get { return _travg; } set { SetValue(ref _travg, value, "travg"); } }

    //    public int size { get; set; }


    //}

    public class SensorSummaryData
    {
        public long t { get; set; }

        public int steps { get; set; }

        public int air { get; set; }

        public double airgmax { get; set; }

        public double airaltmax { get; set; }

        public int turns { get; set; }

        public double tgmax { get; set; }

        public double tgavg { get; set; }

        public double travg { get; set; }
        public int size { get; set; }


    }

    public class SensorAirData
    {
        public long t { get; set; }
        public double dt { get; set; }
        public double alt { get; set; }
        public double g { get; set; }
    }

    public class SensorTurnData
    {
        public long t { get; set; }
        public double dt { get; set; }
        public double r { get; set; }
        public double g { get; set; }
    }

    public class SensorRawData
    {
        public long t { get; set; }
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
