using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Movesense
{
    /// <summary>
    /// Interface for $safeprojectgroupname$
    /// </summary>
    public partial class MovesenseImplementation : IMovesense
    {
        private static MdsWrapper instance = null;
        private static readonly object padlock = new object();

        public string SCHEME_PREFIX => "suunto://";

        public object MdsInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MdsWrapper();
                    }
                    return instance;
                }
            }
        }

        public object Activity { set => new object(); }
    }

    public class MdsWrapper
    { }
}
