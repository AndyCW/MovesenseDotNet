using System;
using System.Threading.Tasks;
using Plugin.Movesense;

namespace MdsLibrary.Api
{
    /// <summary>
    /// Interface for class that allows subscription to a Movesense subscription resource
    /// </summary>
    /// <typeparam name="T">Type of the data object the subscription reports in notifications</typeparam>
    public interface IApiSubscription<T>
    {
        /// <summary>
        /// The Subscription to the resource
        /// </summary>
        IMdsSubscription Subscription { get; }

        /// <summary>
        /// Subscribe to the resource
        /// </summary>
        /// <param name="notificationCallback">Callback function that the MdsLib calls with periodic notifications</param>
        /// <returns></returns>
        Task<IMdsSubscription> SubscribeAsync(Action<T> notificationCallback);
        /// <summary>
        /// Subscribe to the resource. If the initial subscription throws an exception, retry up to two times.
        /// </summary>
        /// <param name="notificationCallback">Callback function that the MdsLib calls with periodic notifications</param>
        /// <returns></returns>
        Task<IMdsSubscription> SubscribeWithRetryAsync(Action<T> notificationCallback);
        void UnSubscribe();
    }
}