using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class IMU6Subscription : ApiSubscription<IMU6Data>
    {
        private static string IMU6_PATH = "Meas/IMU6/";
        private const string DEFAULT_SAMPLE_RATE = "26";
        private string mSampleRate;

        public IMU6Subscription(bool? cancelled, string deviceName, string sampleRate = DEFAULT_SAMPLE_RATE) :
            base(cancelled, deviceName)
        {
            mSampleRate = sampleRate;
        }

        protected override IMdsSubscription subscribe(Mds mds, string serial, IMdsNotificationListener notificationListener)
        {
            return mds.Subscribe(URI_EVENTLISTENER, FormatContractToJson(serial, IMU6_PATH + mSampleRate), notificationListener);
        }
    }
}
