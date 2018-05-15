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
        private static readonly string GYROMETER_PATH = "Meas/Gyro/";
        private const string DEFAULT_SAMPLE_RATE = "26";
        private readonly string mSampleRate;

        /// <summary>
        /// Subscribe to Gyrometer data
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="sampleRate">Sampling rate, e.g. "26" for 26Hz</param>
        public GyrorometerSubscription(string deviceName, string sampleRate = DEFAULT_SAMPLE_RATE) :
            base(deviceName)
        {
            mSampleRate = sampleRate;
        }

        protected override IMdsSubscription subscribe(Mds mds, string serial, IMdsNotificationListener notificationListener)
        {
            return mds.Subscribe(URI_EVENTLISTENER, FormatContractToJson(serial, GYROMETER_PATH + mSampleRate), notificationListener);
        }
    }
}
