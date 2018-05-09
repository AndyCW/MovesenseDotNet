using System;
using ObjCRuntime;

[Native]
public enum MDSResponseMethod : nint
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
public enum MDSRequestMethod : nuint
{
	Unknown,
	Get,
	Post,
	Put,
	Delete,
	Subscribe,
	Unsubscribe
}
