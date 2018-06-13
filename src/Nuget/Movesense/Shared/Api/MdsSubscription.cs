using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="nativeSubscription">Reference to the native MdsLib IMdsSubscription</param>
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
            throw new NotImplementedException();
#endif
        }
    }
}
