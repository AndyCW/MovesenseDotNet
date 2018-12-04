using System;
using ObjCRuntime;

[assembly: LinkWith ("libmds.a", LinkTarget.ArmV7 | LinkTarget.Simulator, ForceLoad = true)]
