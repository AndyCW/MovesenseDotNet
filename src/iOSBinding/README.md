
# Movesense iOS Binding

This folder contains the implementation of the Xamarin bindings project that wraps the native iOS libMDS.a library.

The output from this project has been published on NuGet, but you do not need to reference that package directly in your projects - it is added as a dependency of Plugin.Movesense.

## How to build the MovesenseBindingiOS NuGet package

The output from this solution is packaged in a NuGet package **MovesenseBindingiOS.*version*.nupkg**. These are the steps to build this package:

1. Get the latest version of the Mdslib libraries. These are published by Suunto at <https://bitbucket.org/suunto/movesense-mobile-lib/>. You will find the latest version of the iOS **mdslib** in the *movesense-mobile-lib\IOS\Movesense\Release-iphoneos* folder. It is named, *libmds.a*. 

1. Copy the *libmds.a* and paste it into the **/Resources** folder in this project, replacing the existing version.

1. Edit the MovesenseBindingiOS.csproj file (you'll need to unload the project, then edit the file, then reload the project). Set the following field for the version of the Binding library you want to package:

  ```xml
    <PackageVersion>1.44.0.1</PackageVersion>
    <PackageReleaseNotes>Movesense mdslib v1.44.0 binding for Xamarin iOS.</PackageReleaseNotes>
  ```

1. Now build the project and hope...
   * If you just get warnings, then you're probably OK.
   * If you get errors, then more significant surgery is required.

1. If you get errors, then the Binding project needs some more work. Read [Binding iOS Swift Libraries](https://devblogs.microsoft.com/xamarin/binding-ios-swift-libraries/) for more information.

1. After it builds OK, this will create the new .nupkg in the *MovesenseDotNet\src\iOS\Movesense\bin\Release* folder.

1. The official version will get published to nuget.org at this point, but you can save the NuGet in a local folder for building your own custom version of Movesense.NET.

1. **Next Steps:**
   1. Build the [Android binding](../../AndroidBinding/README.md) project
   1. Build the [Movesense.NET](../../Movesensedotnet/README.md) library
