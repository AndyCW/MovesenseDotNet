using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class IMU9Subscription : ApiSubscription<IMU9Data>
    {
        private static string IMU9_PATH = "Meas/IMU9/";
        private const string DEFAULT_SAMPLE_RATE = "26";
        private string mSampleRate;

        public IMU9Subscription(bool? cancelled, string deviceName, string sampleRate = DEFAULT_SAMPLE_RATE) :
            base(cancelled, deviceName)
        {
            mSampleRate = sampleRate;
        }

        protected override IMdsSubscription subscribe(Mds mds, string serial, IMdsNotificationListener notificationListener)
        {
            return mds.Subscribe(URI_EVENTLISTENER, FormatContractToJson(serial, IMU9_PATH + mSampleRate), notificationListener);
        }
    }
}
