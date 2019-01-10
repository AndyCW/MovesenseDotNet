using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Movesense
{
    /// <summary>
    /// Describes object returned when a subscription is made to a resource
    /// </summary>
    public interface IMdsSubscription
    {
        /// <summary>
        /// Unsubscribe from the subscription
        /// </summary>
        void Unsubscribe();
    }
}
