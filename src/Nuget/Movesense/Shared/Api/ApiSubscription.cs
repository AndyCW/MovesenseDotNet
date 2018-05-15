using Com.Movesense.Mds;
using MdsLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MdsLibrary.Api
{
    public abstract class ApiSubscription<T>
    {
        private static readonly int RETRY_DELAY = 5000; //5 sec
        private static int MAX_RETRY_COUNT = 2;
        private int retries = 0;
        private readonly string mDeviceName;
        private IMdsSubscription mMdsSubscription;
        public static readonly string URI_EVENTLISTENER = "suunto://MDS/EventListener";

        /// <summary>
        /// Base class for API subscriptions
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. "Movesense 174430000051"</param>
        public ApiSubscription(string deviceName)
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

        public async Task<bool> SubscribeWithRetryAsync(Action<T> notificationCallback)
        {
            TaskCompletionSource<bool> retryTcs = new TaskCompletionSource<bool>();
            bool result = true;
            bool doRetry = true;
            while (doRetry)
            {
                try
                {
                    result = await doSubscribe(notificationCallback);
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

        public Task<bool> SubscribeAsync(Action<T> notificationCallback)
        {
            return doSubscribe(notificationCallback);
        }

        public void UnSubscribe()
        {
            Debug.WriteLine("Unsubscribing Mds api subscription");
            mMdsSubscription?.Unsubscribe();
            mMdsSubscription = null;
        }

        private Task<bool> doSubscribe(Action<T> notificationCallback)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            mMdsSubscription = subscribe(
                                    Mdx.MdsInstance,
                                    Util.GetVisibleSerial(mDeviceName),
                                    new MdsNotificationListener(tcs, notificationCallback)
                                    );

            return tcs.Task;
        }

        protected abstract IMdsSubscription subscribe(Com.Movesense.Mds.Mds mds, string serial, IMdsNotificationListener notificationListener);

        protected string FormatContractToJson(string serial, string uri)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"Uri\": \"");
            sb.Append(serial);
            sb.Append("/");
            sb.Append(uri);
            sb.Append("\"}");
            return sb.ToString();
        }

        protected class MdsNotificationListener : Java.Lang.Object, IMdsNotificationListener
        {
            private TaskCompletionSource<bool> mTcs;
            private Action<T> mNotificationCallback;

            public MdsNotificationListener(TaskCompletionSource<bool> tcs, Action<T> notificationCallback)
            {
                mTcs = tcs;
                mNotificationCallback = notificationCallback;
            }

            public void OnError(Com.Movesense.Mds.MdsException e)
            {
                Debug.WriteLine($"ERROR error = {e.ToString()}");
                mTcs.SetException(new MdsException(e.ToString(), e));
            }

            public void OnNotification(string s)
            {
                Debug.WriteLine($"NOTIFICATION data = {s}");
                if (typeof(T) != typeof(String))
                {
                    T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s);
                    mTcs.TrySetResult(true);
                    mNotificationCallback?.Invoke(result);
                }
                else
                {
                    // Crazy code to convert a string to a 'T' where 'T' happens to be a string
                    T result = (T)((object)s);
                    mTcs.TrySetResult(true);
                    mNotificationCallback?.Invoke(result);
                }
            }
        }
    }
}
