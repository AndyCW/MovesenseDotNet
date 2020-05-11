using System;
using ObjCRuntime;

namespace Movesense
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
	public enum MDSRequestMethod : ulong
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
