using Com.Movesense.Mds;
using MdsLibrary.Model;

namespace MdsLibrary.Api
{
    public class MagnetometerSubscription : ApiSubscription<MagnData>
    {
        private static readonly string MAGNETOMETER_PATH = "Meas/Magn/";
        private const string DEFAULT_SAMPLE_RATE = "26";
        private readonly string mSampleRate;

        /// <summary>
        /// Subscribe to Magnetometer data
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        /// <param name="sampleRate">Sampling rate, e.g. "26" for 26Hz</param>
        public MagnetometerSubscription(string deviceName, string sampleRate = DEFAULT_SAMPLE_RATE) :
            base(deviceName)
        {
            mSampleRate = sampleRate;
        }

        protected override IMdsSubscription subscribe(Mds mds, string serial, IMdsNotificationListener notificationListener)
        {
            return mds.Subscribe(URI_EVENTLISTENER, FormatContractToJson(serial, MAGNETOMETER_PATH + mSampleRate), notificationListener);
        }
    }
}
