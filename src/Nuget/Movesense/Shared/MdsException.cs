using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary
{
    public class MdsException : Exception
    {
        public MdsException() { }

        public MdsException(string message, Com.Movesense.Mds.MdsException e) : base(message, e)
        {
        }

        public MdsException(string message) : base(message)
        {
        }
    }
}
