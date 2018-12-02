using MdsLibrary.Helpers;
using Plugin.Movesense;
using Plugin.Movesense.Api;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
#if __ANDROID__
using Com.Movesense.Mds;
#endif

namespace MdsLibrary.Api
{
    /// <summary>
    /// Makes an APICall to MdsLib for those MdsLib methods that do not return data
    /// </summary>
    public class ApiCallAsync
    {
        private static readonly int RETRY_DELAY = 5000; //5 sec
        private static int MAX_RETRY_COUNT = 2;
        private int retries = 0;
        private readonly string mDeviceName;
        private readonly string mPath;
        private readonly string mBody;
        private readonly MdsOp mRestOp;
        private TaskCompletionSource<bool> mTcs = null;

        /// <summary>
        /// Create an ApiCall instance
        /// </summary>
        /// <param name="deviceName">The name of the device e.g. Movesense 908637721113</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        public ApiCallAsync(string deviceName, MdsOp restOp, string path, string body = null)
        {
            mDeviceName = deviceName;
            mPath = path;
            mRestOp = restOp;
            mBody = body;

            // Define the built-in implementation of the retry function
            // This just retries 2 times, regardless of the exception thrown
            // The user may provide their own implementation of the Retry function to override this behavior
            RetryFunction = new Func<Exception, bool?>((Exception ex) =>
            {
                bool? cancel = false;
                if (++retries > MAX_RETRY_COUNT)
                {
                    cancel = true;
                }
                return cancel;
            }
        );
        }

        /// <summary>
        /// Retry function, called after the function call fails.
        /// The built-in implementation retries 2 times, regardless of the exception thrown.
        /// Override the built-in implementation by setting this to your own implementation of the Retry function
        /// </summary>
        public Func<Exception, bool?> RetryFunction;

        /// <summary>
        /// Make the API call (async)
        /// </summary>
        /// <returns>Response object of type T</returns>
        public async Task CallWithRetryAsync()
        {
            TaskCompletionSource<bool> retryTcs = new TaskCompletionSource<bool>();
            bool result = true;
            bool doRetry = true;
            while (doRetry)
            {
                try
                {
                    await perform();
                    retryTcs.SetResult(result);
                    doRetry = false;
                }
                catch (Exception ex)
                {
                    bool? mCancelled = RetryFunction?.Invoke(ex);
                    if (mCancelled.HasValue && mCancelled.Value)
                    {
                        // User has cancelled - break out of loop
                        Debug.WriteLine($"MAX RETRY COUNT EXCEEDED giving up Mds Api Call after exception: {ex.ToString()}");
                        retryTcs.SetException(ex);
                        throw ex;
                    }
                    else
                    {
                        Debug.WriteLine($"RETRYING Mds Api Call after exception: {ex.ToString()}");
                        await Task.Delay(RETRY_DELAY);
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Make the API call (async)
        /// </summary>
        /// <returns>Response object of type T</returns>
        public Task CallAsync()
        {
            return perform();
        }

        private Task perform()
        {
            mTcs = new TaskCompletionSource<bool>();

#if __ANDROID__
            var mds = (Com.Movesense.Mds.Mds)CrossMovesense.Current.MdsInstance;
            var serial = Util.GetVisibleSerial(mDeviceName);
            if (mRestOp == MdsOp.POST)
            {
                mds.Post(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + mPath, mBody, new MdsResponseListener(mTcs));
            }
            else if (mRestOp == MdsOp.GET)
            {
                mds.Get(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + mPath, null, new MdsResponseListener(mTcs));
            }
            else if (mRestOp == MdsOp.DELETE)
            {
                mds.Delete(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + mPath, null, new MdsResponseListener(mTcs));
            }
            else if (mRestOp == MdsOp.PUT)
            {
                mds.Put(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + mPath, mBody, new MdsResponseListener(mTcs));
            }
#endif
            return mTcs.Task;
        }

        /// <summary>
        /// MdsResponseListener called by MdsLib with result of the call (Internal)
        /// </summary>
        protected class MdsResponseListener
#if __ANDROID__
            : Java.Lang.Object, IMdsResponseListener
#endif
        {
            private TaskCompletionSource<bool> mTcs;

            /// <summary>
            /// Response LIstener class contains error and success callbacks for a call to Mds
            /// </summary>
            /// <param name="tcs">TaskCompletionSource used for handling cancellation</param>
            public MdsResponseListener(TaskCompletionSource<bool> tcs)
            {
                mTcs = tcs;
            }

            /// <summary>
            /// Callback on success receives response as a Json string
            /// </summary>
            /// <param name="s">response as a Json string</param>
#if __ANDROID__
            /// <param name="mdsHeader">Header object with details of the call</param>
            public void OnSuccess(string s, MdsHeader mdsHeader)
#else
            public void OnSuccess(string s)
#endif
            {
                Debug.WriteLine($"SUCCESS result = {s}");
                mTcs.SetResult(true);
            }

            /// <summary>
            /// Error callback
            /// </summary>
            /// <param name="e">exception containing details of the error</param>
#if __ANDROID__
            public void OnError(Com.Movesense.Mds.MdsException e)
            {
                Debug.WriteLine($"ERROR error = {e.ToString()}");
                mTcs.SetException(new MdsException(e.ToString(), e));
            }
#elif __IOS__
            public void OnError(Exception e)
            {
                Debug.WriteLine($"ERROR error = {e.ToString()}");
                mTcs.SetException(new MdsException(e.ToString(), e));
            }
#endif
        }
    }
}
