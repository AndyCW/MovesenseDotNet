using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class IMU9Subscription : ApiSubscription<IMU9Data>
    {
        private static readonly string IMU9_PATH = "Meas/IMU9/";
        private const string DEFAULT_SAMPLE_RATE = "26";
        private readonly string mSampleRate;

        /// <summary>
        /// Subscribe to IMU9 data
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="sampleRate">Sampling rate, e.g. "26" for 26Hz</param>
        public IMU9Subscription(string deviceName, string sampleRate = DEFAULT_SAMPLE_RATE) :
            base(deviceName)
        {
            mSampleRate = sampleRate;
        }

        protected override IMdsSubscription subscribe(Mds mds, string serial, IMdsNotificationListener notificationListener)
        {
            return mds.Subscribe(URI_EVENTLISTENER, FormatContractToJson(serial, IMU9_PATH + mSampleRate), notificationListener);
        }
    }
}
