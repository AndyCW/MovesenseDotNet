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
        private static MdsWrapper instance = null;
        private static readonly object padlock = new object();

        public string SCHEME_PREFIX => "suunto://";

        public object MdsInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MdsWrapper();
                    }
                    return instance;
                }
            }
        }

        public object Activity { set => new object(); }

        /// <summary>
        /// Connect a device to MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>null</returns>
        public Task<object> ConnectMdsAsync(Guid Uuid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>null</returns>
        public Task<object> DisconnectMds(Guid Uuid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function to make Mds API call that does not return a value
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        public Task ApiCallAsync(string deviceName, MdsOp restOp, string path, string body = null)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Function to make Mds API call that returns a value of type T
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        public Task<T> ApiCallAsync<T>(string deviceName, MdsOp restOp, string path, string body = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function to start a subscription to an Mds resource
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="frequency">Sample rate, e.g. 52 for 52Hz</param>
        /// <param name="notificationCallback">Callback function that takes parameter of type T, where T is the return type from the subscription notifications</param>
        public async Task<IMdsSubscription> ApiSubscriptionAsync<T>(string deviceName, string path, int frequency, Action<T> notificationCallback)

        {
            throw new NotImplementedException();
        }
    }

    public class MdsWrapper
    { }
}
