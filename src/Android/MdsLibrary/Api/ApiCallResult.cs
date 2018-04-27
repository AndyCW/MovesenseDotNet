using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary.Api
{
    public interface ApiCallResult<T>
    {
        void OnResult(T result);
    }
}
