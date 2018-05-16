using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Movesense
{
    public interface IMovesense
    {
        object MdsInstance { get; }
        object Activity { set; }
        string SCHEME_PREFIX { get; }
    }
}
