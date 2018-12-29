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
#if __IOS__
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
        /// <returns>MdsConnectionContext contains device IDs. Pass this object in all other Movesense.NET API calls to specify target device.</returns>
        public async Task<MdsConnectionContext> ConnectMdsAsync(Guid Uuid)
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
        [Obsolete("DisconnectMdsAsync(Guid) is deprecated, please use DisconnectMdsAsync(MdsConnectionContext) instead.")]
        public async Task<object> DisconnectMdsAsync(Guid Uuid)
        {
            return await new MdsConnectionService().DisconnectMdsAsync(Uuid.ToString());
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="mdsConnectionContext">MdsConnectionContext of the device</param>
        /// <returns>null</returns>
        public async Task<object> DisconnectMdsAsync(MdsConnectionContext mdsConnectionContext)
        {
            return await new MdsConnectionService().DisconnectMdsAsync(mdsConnectionContext.Uuid.ToString());
        }
    }
#endif
}