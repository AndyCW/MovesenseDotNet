using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorKitSDK
{
    public interface IConnector
    {
        bool IsConnected { get; set; }
        Task Subscribe();
        Task Unsubscribe();
    }
}
