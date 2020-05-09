using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if __IOS__
using Movesense;
#endif

namespace Plugin.Movesense.Api
{
    /// <summary>
    /// Contains context for a subscription to an MdsLib subscription resource
    /// </summary>
    public class MdsSubscription : IMdsSubscription
    {
        private object mNativeSubscription;

        /// <summary>
        /// Creates a context for a subscription to an MdsLib subscription resource
        /// </summary>
        /// <param name="nativeSubscription">(Android)Reference to the native MdsLib IMdsSubscription, (iOS)path to the subscrfiption for the device</param>
        public MdsSubscription(object nativeSubscription)
        {
            mNativeSubscription = nativeSubscription;
        }

        /// <summary>
        /// Unsubscribe from the resource
        /// </summary>
        public void Unsubscribe()
        {
#if __ANDROID__
            ((Com.Movesense.Mds.IMdsSubscription)mNativeSubscription).Unsubscribe();
#elif __IOS__
            var mds = (MDSWrapper)CrossMovesense.Current.MdsInstance;
            // On iOS, the 'nativeSubscription' refers to the Uri for the subscription
            string path = (string)mNativeSubscription;
            mds.DoUnsubscribe(path);
#endif
        }
    }
}
