using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary
{
    public class MdsException : Exception
    {
        public MdsException() { }

#if __ANDROID__
        public MdsException(string message, Com.Movesense.Mds.MdsException e) : base(message, e)
        {
        }
#elif __IOS__
        public MdsException(string message, Exception e) : base(message, e)
        {
        }
#endif

        public MdsException(string message) : base(message)
        {
        }
    }
}
