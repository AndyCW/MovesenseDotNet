#if __ANDROID__
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
        /// <returns>MdsMovesenseDevice object for the device.</returns>
        public async Task<IMovesenseDevice> ConnectMdsAsync(Guid Uuid)
        {
            return await new MdsConnectionService().ConnectMdsAsync(Uuid).ConfigureAwait(false);
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>null</returns>
        [Obsolete("DisconnectMdsAsync(Guid) is deprecated, please use DisconnectMdsAsync(MdsConnectionContext) instead.")]
        public Task<object> DisconnectMdsAsync(Guid Uuid)
        {
            return new MdsConnectionService().DisconnectMdsAsync(Uuid);
        }

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="mdsDevice">IMovesenseDevice of the device</param>
        /// <returns>null</returns>
        public async Task<object> DisconnectMdsAsync(IMovesenseDevice mdsDevice)
        {
            return await new MdsConnectionService().DisconnectMdsAsync(mdsDevice.Uuid).ConfigureAwait(false);
        }
    }
}
#endif