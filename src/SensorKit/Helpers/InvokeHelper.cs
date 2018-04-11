using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SensorKitSDK
{
    public class InvokeHelper
    {
        // assuming the static initializer is executed on the UI Thread.
        public static SynchronizationContext mainSyncronisationContext = SynchronizationContext.Current;

        public static void Invoke(Action action)
        {
            mainSyncronisationContext?.Post(_ => action(), null);
        }
    }
}
