using Com.Movesense.Mds;
using MdsLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MdsLibrary.Api
{
    public abstract class ApiCallAsync<T>
    {
        private static int RETRY_DELAY = 5000; //5 sec
        private static int MAX_RETRY_COUNT = 2;
        private int retries = 0;
        private bool? mCancelled;
        private string mDeviceName;

        public ApiCallAsync(bool? cancelled, string deviceName)
        {
            mCancelled = cancelled;
            mDeviceName = deviceName;

            mRetry = new Func<bool?>(() =>
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

        public Func<bool?> mRetry;

        public async Task<T> PerformWithRetryAsync()
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
                    mCancelled = mRetry?.Invoke();
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

        public Task<T> PerformAsync()
        {
            return perform();
        }

        private Task<T> perform()
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            performCall(
                Mdx.MdsInstance,
                Util.GetVisibleSerial(mDeviceName),
                new MdsResponseListener(mCancelled, tcs)
                );

            return tcs.Task;
        }

        protected abstract void performCall(Com.Movesense.Mds.Mds mds, string serial, IMdsResponseListener responseListener);

        protected class MdsResponseListener : Java.Lang.Object, IMdsResponseListener
        {
            private bool? mCancelled;
            private TaskCompletionSource<T> mTcs;

            public MdsResponseListener(bool? cancelled, TaskCompletionSource<T> tcs)
            {
                mCancelled = cancelled;
                mTcs = tcs;
            }


            public void OnSuccess(string s)
            {
                Debug.WriteLine($"SUCCESS result = {s}");
                if (mCancelled.HasValue && !mCancelled.Value)
                {
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
            }

            public void OnError(Com.Movesense.Mds.MdsException e)
            {
                Debug.WriteLine($"ERROR error = {e.ToString()}");
                mTcs.SetException(new MdsException(e.ToString(), e));
            }
        }
    }

}
