using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class GyrorometerSubscription : ApiSubscription<GyroData>
    {
        private static string GYROMETER_PATH = "Meas/Gyro/";
        private const string DEFAULT_SAMPLE_RATE = "26";
        private string mSampleRate;

        public GyrorometerSubscription(bool? cancelled, string deviceName, string sampleRate = DEFAULT_SAMPLE_RATE) :
            base(cancelled, deviceName)
        {
            mSampleRate = sampleRate;
        }

        protected override IMdsSubscription subscribe(Mds mds, string serial, IMdsNotificationListener notificationListener)
        {
            return mds.Subscribe(URI_EVENTLISTENER, FormatContractToJson(serial, GYROMETER_PATH + mSampleRate), notificationListener);
        }
    }
}
