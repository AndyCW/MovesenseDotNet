using Com.Movesense.Mds;
using MdsLibrary.Helpers;
using Plugin.Movesense;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MdsLibrary.Api
{
    public abstract class ApiCallAsync<T>
    {
        private static readonly int RETRY_DELAY = 5000; //5 sec
        private static int MAX_RETRY_COUNT = 2;
        private int retries = 0;
        private readonly string mDeviceName;

        /// <summary>
        /// Base class for all Mds API calls
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public ApiCallAsync(string deviceName)
        {
            mDeviceName = deviceName;

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

            performCall(
                (Com.Movesense.Mds.Mds)CrossMovesense.Current.MdsInstance,
                Util.GetVisibleSerial(mDeviceName),
                new MdsResponseListener(tcs)
                );

            return tcs.Task;
        }

        protected abstract void performCall(Com.Movesense.Mds.Mds mds, string serial, IMdsResponseListener responseListener);

        protected class MdsResponseListener : Java.Lang.Object, IMdsResponseListener
        {
            private TaskCompletionSource<T> mTcs;

            public MdsResponseListener(TaskCompletionSource<T> tcs)
            {
                mTcs = tcs;
            }

            public void OnSuccess(string s)
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

            public void OnError(Com.Movesense.Mds.MdsException e)
            {
                Debug.WriteLine($"ERROR error = {e.ToString()}");
                mTcs.SetException(new MdsException(e.ToString(), e));
            }
        }
    }

}
