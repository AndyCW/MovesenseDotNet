#if __IOS__
using System;

namespace MdsLibrary.Api
{
    public interface IMdsResponseListener :IDisposable
    {
        void OnError(MdsException p0);
        void OnSuccess(string p0);
    }
}
#endif