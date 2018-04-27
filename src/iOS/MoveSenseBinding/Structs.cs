using System;
using Foundation;
using MoveSenseBinding;
using ObjCRuntime;

namespace MoveSenseBinding
{
[Native]
public enum MDSResponseMethod : long
{
    Unknown,
    Get,
    Post,
    Put,
    Delete,
    Subscribe,
    Unsubscribe,
    Register,
    Unregister
}

[Native]
public enum MDSRequestMethod : long
{
    Unknown,
    Get,
    Post,
    Put,
    Delete,
    Subscribe,
    Unsubscribe
}

}
