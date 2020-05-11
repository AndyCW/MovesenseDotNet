using System;
using System.Collections.Generic;
using System.Text;

namespace MdsLibrary
{
    /// <summary>
    /// Custom exception encapsulates native exceptions thrown from within MdsLib
    /// </summary>
    public class MdsException : Exception
    {
        /// <summary>
        /// MdsException constructor
        /// </summary>
        public MdsException() { }

#if __ANDROID__
        /// <summary>
        /// Creates an MdsException wrapping a Com.Movesense.Mds.MdsException thrown by the native Android MdsLib
        /// /// </summary>
        /// <param name="message">message string</param>
        /// <param name="e">the exception to wrap</param>
        public MdsException(string message, Com.Movesense.Mds.MdsException e) : base(message, e)
        {
        }
#elif __IOS__
        /// <summary>
        /// Creates an MdsException wrapping another exception
        /// </summary>
        /// <param name="message">message string</param>
        /// <param name="e">the exception to wrap</param>
        public MdsException(string message, Exception e) : base(message, e)
        {
        }
#endif

        /// <summary>
        /// Creates an MdsException
        /// </summary>
        /// <param name="message">exception message string</param>
        public MdsException(string message) : base(message)
        {
        }
    }
}
