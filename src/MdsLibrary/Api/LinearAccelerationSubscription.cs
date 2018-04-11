using Com.Movesense.Mds;
using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public class LinearAccelerationSubscription : ApiSubscription<LinearAcceleration>
    {
        private static string LINEAR_ACCELERATION_PATH = "Meas/Acc/";
        private static string SAMPLE_RATE = "52";

        public LinearAccelerationSubscription(bool? cancelled, string deviceName) :
            base(cancelled, deviceName)
        {
        }

        protected override IMdsSubscription subscribe(Mds mds, string serial, IMdsNotificationListener notificationListener)
        {
            return mds.Subscribe(URI_EVENTLISTENER, FormatContractToJson(serial, LINEAR_ACCELERATION_PATH + SAMPLE_RATE), notificationListener);
        }
    }
}
