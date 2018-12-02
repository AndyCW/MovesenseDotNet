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
    /// Makes an APICall to MdsLib for those MdsLib methods that return data of type T
    /// </summary>
    public class ApiCallAsync<T>
    {
        private static readonly int RETRY_DELAY = 5000; //5 sec
        private static int MAX_RETRY_COUNT = 2;
        private int retries = 0;
        private readonly string mDeviceName;
        private readonly string mPath;
        private readonly string mBody;
        private readonly MdsOp mRestOp;

        /// <summary>
        /// Base class for all Mds API calls
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
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
        /// Make the API call (async) with up to two retries if the call throws an exception.
        /// Set RetryFunction property to override the builtin retry logic.
        /// </summary>
        /// <returns>Response object of type T</returns>
        public async Task<T> CallWithRetryAsync()
        {
            TaskCompletionSource<T> retryTcs = new TaskCompletionSource<T>();
            T result = default(T);
            bool doRetry = true;
            while (doRetry)
            {
                try
                {
                    result = await perform();
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

            return result;
        }

        /// <summary>
        /// Make the API call (async)
        /// </summary>
        /// <returns>Response object of type T</returns>
        public Task<T> CallAsync()
        {
            return perform();
        }

        private Task<T> perform()
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

#if __ANDROID__
            var mds = (Com.Movesense.Mds.Mds)CrossMovesense.Current.MdsInstance;
            var serial = Util.GetVisibleSerial(mDeviceName);
            if (mRestOp == MdsOp.POST)
            {
                mds.Post(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + mPath, mBody, new MdsResponseListener(tcs));
            }
            else if (mRestOp == MdsOp.GET)
            {
                mds.Get(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + mPath, null, new MdsResponseListener(tcs));
            }
            else if (mRestOp == MdsOp.DELETE)
            {
                mds.Delete(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + mPath, null, new MdsResponseListener(tcs));
            }
            else if (mRestOp == MdsOp.PUT)
            {
                mds.Put(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + serial + mPath, mBody, new MdsResponseListener(tcs));
            }
#elif __IOS__
            throw new NotImplementedException();
#endif
            return tcs.Task;
        }

        /// <summary>
        /// MdsResponseListener called by MdsLib with result of the call (Internal)
        /// </summary>
        protected class MdsResponseListener
#if __ANDROID__
            : Java.Lang.Object, IMdsResponseListener
#endif
        {
            private TaskCompletionSource<T> mTcs;

            /// <summary>
            /// Response Listener class contains error and success callbacks for a call to Mds
            /// </summary>
            /// <param name="tcs">TaskCompletionSource used for handling cancellation</param>
            public MdsResponseListener(TaskCompletionSource<T> tcs)
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
                if (typeof(T) != typeof(String))
                {
                    T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s);
                    mTcs.SetResult(result);
                }
                else
                {
                    // Crazy code to convert a string to a 'T' where 'T' happens to be a string
                    T result = (T)((object)s);
                    mTcs.SetResult(result);
                }
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
