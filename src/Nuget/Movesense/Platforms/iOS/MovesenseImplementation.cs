using MdsLibrary;
using MdsLibrary.Api;
using Movesense;
using Plugin.Movesense.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Movesense
{
    /// <summary>
    /// Interface for $safeprojectgroupname$
    /// </summary>
    public partial class MovesenseImplementation : IMovesense
    {
        private static MDSWrapper instance = null;
        private static readonly object padlock = new object();

        /// <summary>
        /// root of all Uris on the Mds whiteboard
        /// </summary>
        public string SCHEME_PREFIX => "suunto://";

        /// <summary>
        /// Get the singleton instance of the MdsWrapper
        /// </summary>
        public object MdsInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MDSWrapper();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Property exists for compatibility with Android API, but is  a no-op here
        /// </summary>
        public object Activity { set => new object(); }

        /// <summary>
        /// Connect a device to MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>null</returns>
        public async Task<object> ConnectMdsAsync(Guid Uuid)
        {
            // Ensure the listener is initialized
            await MdsConnectionListener.Current.EnsureInitializedAsync();
            // Connect the device
            return await new MdsConnectionService().ConnectMdsAsync(Uuid.ToString());
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>null</returns>
        public async Task<object> DisconnectMdsAsync(Guid Uuid)
        {
            return await new MdsConnectionService().DisconnectMdsAsync(Uuid.ToString());
        }

        /// <summary>
        /// Function to make Mds API call that does not return a value
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        public async Task ApiCallAsync(string deviceName, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            await new ApiCallAsync(deviceName, restOp, path, body, prefixPath).CallAsync();
        }

        /// <summary>
        /// Function to make Mds API call that returns a value of type T
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        public async Task<T> ApiCallAsync<T>(string deviceName, MdsOp restOp, string path, string body = null, string prefixPath = "")
        {
            return await new ApiCallAsync<T>(deviceName, restOp, path, body, prefixPath).CallAsync();
        }

        /// <summary>
        /// Function to start a subscription to an Mds resource
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="notificationCallback">Callback function that takes parameter of type T, where T is the return type from the subscription notifications</param>
        public async Task<IMdsSubscription> ApiSubscriptionAsync<T>(string deviceName, string path, Action<T> notificationCallback)
        {
            return await new ApiSubscription<T>(deviceName, path).SubscribeAsync(notificationCallback);
        }
    }
}
