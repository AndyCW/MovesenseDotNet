using System.Threading.Tasks;
using Plugin.Movesense;

namespace MdsLibrary
{
    /// <summary>
    /// Methods of an MdsConnectionService
    /// </summary>
    public interface IMdsConnectionService
    {
        /// <summary>
        /// Connect to a device
        /// </summary>
        /// <param name="UniqueId">ID - MAC address on Android, UUid on iOS</param>
        /// <returns></returns>
        Task<IMovesenseDevice> ConnectMdsAsync(string UniqueId);

        /// <summary>
        /// Disconnect a device from MdsLIb
        /// </summary>
        /// <param name="UniqueId">ID - MAC address on Android, UUid on iOS</param>
        /// <returns></returns>
        Task<object> DisconnectMdsAsync(string UniqueId);
    }
}