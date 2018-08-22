using MdsLibrary.Helpers;
using Plugin.Movesense;
using Plugin.Movesense.Api;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
#if __ANDROID__
using Com.Movesense.Mds;
#endif
#if __IOS__
using Foundation;
#endif

namespace MdsLibrary.Api
{
    /// <summary>
    /// Makes an APICall to MdsLib for those MdsLib methods that do not return data
    /// </summary>
    public class ApiCallAsync
#if __ANDROID__
            : Java.Lang.Object, IMdsResponseListener
#endif
    {
        private static readonly int RETRY_DELAY = 5000; //5 sec
        private static int MAX_RETRY_COUNT = 2;
        private int retries = 0;
        private readonly string mDeviceName;
        private readonly string mPath;
        private readonly string mBody;
        private readonly MdsOp mRestOp;
        private readonly string mPrefixPath;
        private TaskCompletionSource<bool> mTcs = null;

        /// <summary>
        /// Create an ApiCall instance
        /// </summary>
        /// <param name="deviceName">The name of the device e.g. Movesense 908637721113</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        public ApiCallAsync(string deviceName, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            mDeviceName = deviceName;
            mPath = path;
            mRestOp = restOp;
            mBody = body;
            mPrefixPath = prefixPath;

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
                mds.Post(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + mPrefixPath + serial + mPath, mBody, this);
            }
            else if (mRestOp == MdsOp.GET)
            {
                mds.Get(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + mPrefixPath + serial + mPath, null, this);
            }
            else if (mRestOp == MdsOp.DELETE)
            {
                mds.Delete(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + mPrefixPath + serial + mPath, null, this);
            }
            else if (mRestOp == MdsOp.PUT)
            {
                mds.Put(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + mPrefixPath + serial + mPath, mBody, this);
            }
#endif

#if __IOS__
            var mds = (Movesense.MDSWrapper)CrossMovesense.Current.MdsInstance;
            var serial = Util.GetVisibleSerial(mDeviceName);
            NSDictionary bodyDict = new NSDictionary();

            if (mRestOp == MdsOp.POST)
            {
                if (!string.IsNullOrEmpty(mBody))
                {
                    NSData data = NSData.FromString(mBody);
                    NSError error = new NSError();
                    bodyDict = (NSDictionary)NSJsonSerialization.Deserialize(data, NSJsonReadingOptions.MutableContainers, out error);
                }
                mds.DoPost(mPrefixPath + serial + mPath, contract: bodyDict, completion: (arg0) => CallCompletionCallback(arg0));
            }
            else if (mRestOp == MdsOp.GET)
            {
                mds.DoGet(mPrefixPath + serial + mPath, contract: bodyDict, completion: (arg0) => CallCompletionCallback(arg0));
            }
            else if (mRestOp == MdsOp.DELETE)
            {
                mds.DoDelete(mPrefixPath + serial + mPath, contract: bodyDict, completion: (arg0) => CallCompletionCallback(arg0));
            }
            else if (mRestOp == MdsOp.PUT)
            {
                if (!string.IsNullOrEmpty(mBody))
                {
                    NSData data = NSData.FromString(mBody);
                    NSError error = new NSError();
                    bodyDict = (NSDictionary)NSJsonSerialization.Deserialize(data, NSJsonReadingOptions.MutableContainers, out error);
                }
                mds.DoPut(mPrefixPath + serial + mPath, bodyDict, completion: (arg0) => CallCompletionCallback(arg0));
            }
#endif
            return mTcs.Task;
        }


#if __ANDROID__

#region IMdsResponseListener implementation

        /// <summary>
        /// Success callback for API calls on Android
        /// </summary>
        /// <param name="s">return value as string</param>
        public void OnSuccess(string s)
            {
                Debug.WriteLine($"SUCCESS result = {s}");
                mTcs.SetResult(true);
            }

        /// <summary>
        /// Error callback for API calls on Android
        /// </summary>
        /// <param name="e">exception</param>
            public void OnError(Com.Movesense.Mds.MdsException e)
            {
                Debug.WriteLine($"ERROR error = {e.ToString()}");
                mTcs.SetException(new MdsException(e.ToString(), e));
            }
#endregion

#elif __IOS__
        /// <summary>
        /// Callback for MDS API calls on iOS
        /// </summary>
        /// <param name="completion"></param>
        public void CallCompletionCallback(Movesense.MDSResponse completion)
        {
            if (completion.StatusCode == 200)
            {
                // Success
                mTcs.SetResult(true);
            }
            else
            {
                Debug.WriteLine($"ERROR error = {completion.Description}");
                mTcs.SetException(new MdsException(completion.Description));
            }
        }
#endif
    }
}
