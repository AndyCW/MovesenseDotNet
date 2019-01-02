using MdsLibrary.Helpers;
using Plugin.Movesense;
using Plugin.Movesense.Api;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
#if __ANDROID__
using Com.Movesense.Mds;
#endif
#if __IOS__
using Foundation;
#endif

namespace MdsLibrary.Api
{
    /// <summary>
    /// Makes an APICall to MdsLib for those MdsLib methods that return data of type T
    /// </summary>
    public class ApiCallAsync<T>
#if __ANDROID__
            : Java.Lang.Object, IMdsResponseListener
#endif
    {
        private static readonly int RETRY_DELAY = 5000; //5 sec
        private static int MAX_RETRY_COUNT = 2;
        private int retries = 0;
        private readonly string mSerial;
        private readonly string mPath;
        private readonly string mBody;
        private readonly MdsOp mRestOp;
        private readonly string mPrefixPath;
        private TaskCompletionSource<T> mTcs = null;

        /// <summary>
        /// Helper class for all Mds API calls
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use ApiCallAsync<T>(MdsConnectionContext...) constructor instead.")]
        public ApiCallAsync(string deviceName, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            mSerial = Util.GetVisibleSerial(deviceName);
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
        /// Helper class for all Mds API calls
        /// </summary>
        /// <param name="movesenseDevice">IMovesenseDevice for the device</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        public ApiCallAsync(IMovesenseDevice movesenseDevice, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            mSerial = movesenseDevice.Serial;
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
            mTcs = new TaskCompletionSource<T>();

#if __ANDROID__
            var mds = (Com.Movesense.Mds.Mds)CrossMovesense.Current.MdsInstance;
            if (mRestOp == MdsOp.POST)
            {
                mds.Post(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + mPrefixPath + mSerial + mPath, mBody, this);
            }
            else if (mRestOp == MdsOp.GET)
            {
                mds.Get(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + mPrefixPath + mSerial + mPath, mBody, this);
            }
            else if (mRestOp == MdsOp.DELETE)
            {
                mds.Delete(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + mPrefixPath + mSerial + mPath, mBody, this);
            }
            else if (mRestOp == MdsOp.PUT)
            {
                mds.Put(Plugin.Movesense.CrossMovesense.Current.SCHEME_PREFIX + mPrefixPath + mSerial + mPath, mBody, this);
            }
#elif __IOS__
            var mds = (Movesense.MDSWrapper)CrossMovesense.Current.MdsInstance;
            NSDictionary bodyDict = new NSDictionary();
            if (mRestOp == MdsOp.POST)
            {
                if (!string.IsNullOrEmpty(mBody))
                {
                    NSData data = NSData.FromString(mBody);
                    NSError error = new NSError();
                    bodyDict = (NSDictionary)NSJsonSerialization.Deserialize(data, NSJsonReadingOptions.MutableContainers, out error);
                }
                mds.DoPost(mPrefixPath + mSerial + mPath, contract: bodyDict, completion: (arg0) => CallCompletionCallback(arg0));
            }
            else if (mRestOp == MdsOp.GET)
            {
                mds.DoGet(mPrefixPath + mSerial + mPath, contract: bodyDict, completion: (arg0) => CallCompletionCallback(arg0));
            }
            else if (mRestOp == MdsOp.DELETE)
            {
                mds.DoDelete(mPrefixPath + mSerial + mPath, contract: bodyDict, completion: (arg0) => CallCompletionCallback(arg0));
            }
            else if (mRestOp == MdsOp.PUT)
            {
                if (!string.IsNullOrEmpty(mBody))
                {
                    NSData data = NSData.FromString(mBody);
                    NSError error = new NSError();
                    bodyDict = (NSDictionary)NSJsonSerialization.Deserialize(data, NSJsonReadingOptions.MutableContainers, out error);
                }
                mds.DoPut(mPrefixPath + mSerial + mPath, contract: bodyDict, completion: (arg0) => CallCompletionCallback(arg0));
            }
#endif
            return mTcs.Task;
        }


#if __ANDROID__
        /// <summary>
        /// Callback on success receives response as a Json string
        /// </summary>
        /// <param name="s">response as a Json string</param>
        /// <param name="mdsHeader">Header object with details of the call</param>
        public void OnSuccess(string s, MdsHeader mdsHeader)
#else
        /// <summary>
        /// Callback on success receives response as a Json string
        /// </summary>
        /// <param name="s">response as a Json string</param>
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

#if __ANDROID__
        /// <summary>
        /// Error callback
        /// </summary>
        /// <param name="e">exception containing details of the error</param>
        public void OnError(Com.Movesense.Mds.MdsException e)
        {
            Debug.WriteLine($"ERROR error = {e.ToString()}");
            mTcs.SetException(new MdsException(e.ToString(), e));
        }

#elif __IOS__
        /// <summary>
        /// Callback for MDS API calls on iOS
        /// </summary>
        /// <param name="completion"></param>
        public void CallCompletionCallback(Movesense.MDSResponse completion)
        {
            if (completion.StatusCode == 200)
            {
                var data = completion.BodyData;
                NSString s = new NSString(data, NSStringEncoding.UTF8);

                Debug.WriteLine($"SUCCESS result = {s}");
                if (typeof(T) != typeof(String))
                {
                    T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s);
                    mTcs.SetResult(result);
                }
                else
                {
                    String netS = string.Empty;
                    netS = s?.ToString();
                    // Crazy code to convert a string to a 'T' where 'T' happens to be a string
                    T result = (T)((object)netS);
                    mTcs.SetResult(result);
                }
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

