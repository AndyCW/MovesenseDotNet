using Com.Movesense.Mds;
using MdsLibrary;
using MdsLibrary.Api;
using MdsLibrary.Model;
using Plugin.Movesense.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Movesense
{
    /// <summary>
    /// Implementation for the IMovesense plugin access interface
    /// </summary>
    public partial class MovesenseImplementation : IMovesense
    {
        private static Mds instance = null;
        private static readonly object padlock = new object();

        private static Android.App.Activity AndroidActivity = null;

        /// <summary>
        /// Root of all paths to Movesense resources.
        /// </summary>
        public string SCHEME_PREFIX => "suunto://";

        /// <summary>
        /// Gets the native MdsLib object
        /// </summary>
        public object MdsInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        if (AndroidActivity == null)
                        {
                            throw new InvalidOperationException("Set Plugin.Movesense.CrossMovesense.Current.Activity to current Android.App.Activity before calling MdsInstance");
                        }

                        instance = new Mds.Builder().Build(AndroidActivity);
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// On Android, Activity must be set to the current Android Activity before first access of the library.
        /// </summary>
        public object Activity { set => MovesenseImplementation.AndroidActivity = value as Android.App.Activity; }

        /// <summary>
        /// Connect a device to MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>null</returns>
        public async Task<object> ConnectMdsAsync(Guid Uuid)
        {
            return await new MdsConnectionService().ConnectMdsAsync(GetMACAddressFromUuid(Uuid));
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>null</returns>
        public Task<object> DisconnectMds(Guid Uuid)
        {
            return new MdsConnectionService().DisconnectMds(GetMACAddressFromUuid(Uuid));
        }

        /// <summary>
        /// Function to make Mds API call that does not return a value
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        public async Task ApiCallAsync(string deviceName, MdsOp restOp, string path, string body = null)
        {
            await new ApiCallAsync(deviceName, restOp, path, body).CallAsync();
        }
        /// <summary>
        /// Function to make Mds API call that returns a value of type T
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        public async Task<T> ApiCallAsync<T>(string deviceName, MdsOp restOp, string path, string body = null)
        {
            return await new ApiCallAsync<T>(deviceName, restOp, path, body).CallAsync();
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
            return await new ApiSubscription<T>(deviceName, path, frequency).SubscribeAsync(notificationCallback);
        }

        private string GetMACAddressFromUuid(Guid Uuid)
        {
            string[] idParts = Uuid.ToString().Split(new char[] { '-' });
            string macAddress = idParts.Last().ToUpper();
            StringBuilder formattedMAC = new StringBuilder();
            for (int i = 0; i < macAddress.Length; i += 2)
            {
                if (i > 0) formattedMAC.Append(":");
                formattedMAC.Append(macAddress.Substring(i, 2));
            }

            return formattedMAC.ToString();
        }
    }
}
