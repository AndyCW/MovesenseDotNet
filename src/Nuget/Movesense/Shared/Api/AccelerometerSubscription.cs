#if __ANDROID__
using Com.Movesense.Mds;

using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class AccelerometerSubscription : ApiSubscription<AccData>
    {
        private static string ACCELEROMETER_PATH = "Meas/Acc/";
        private const string DEFAULT_SAMPLE_RATE = "26";
        private string mSampleRate;

        public AccelerometerSubscription(bool? cancelled, string deviceName, string sampleRate = DEFAULT_SAMPLE_RATE) :
            base(cancelled, deviceName)
        {
            mSampleRate = sampleRate;
        }

        protected override IMdsSubscription subscribe(Mds mds, string serial, IMdsNotificationListener notificationListener)
        {
            return mds.Subscribe(URI_EVENTLISTENER, FormatContractToJson(serial, ACCELEROMETER_PATH + mSampleRate), notificationListener);
        }
    }
}
#endif