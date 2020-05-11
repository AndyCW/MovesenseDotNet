# Movesense.NET

This folder contains the source for the Movesense.NET library. The project is configured to package the library as a NuGet package ready for deploying in your own local NuGet library.

Contributions to maintaining this code are encouraged!

## Overview

The source for the Movesense.NET library is in project *Movesense*. This project is a multi-target project, configured to build dlls for Xamarin Android 8.0, Xamarin iOS 10, .NET Standard 1.0 and .NET Standard 2.0.

Key points:

* The public API surface of Movesense.NET is defined in files in the *Shared* folder. Included in here is *CrossMovesense.cs* which contains the lazy loader to load the platform-specific Movesense.dll - integral to how a Xamarin plugin works.
* *MdsMovesenseDevice.cs* implements IMovesenseDevice and is the public API users call to perform any operation with a Movesense sensor.
* Platform-specific code is in two areas:
  * Sensor connection/disconnection: in the */Platforms/Android* and */Platforms/iOS* folders
  * Utility calls that call to the Movesense whiteboard:

    * */Shared/API/ApiCallAsync.cs*
    * */Shared/API/ApiCallAsyncOfT.cs*
    * */Shared/API/ApiSubscription.cs*

## Building Movesense.NET

### To change the NuGet package version

To set your own version number for the NuGet package you build for your own testing, do the following:

* In Visual Studio, right-click on the Movesense project, and then click **Edit Project File**
* Change the **\<PackageVersion\>** value:

   ```xml
   <PackageVersion>2.44.0.4</PackageVersion>
   ```

### To change the mdslib bindings referenced by the Movesensedotnet library

Crucial to this library are the .NET bindings around the native mdslib libraries shipped by Movesense/Suunto. See the **/src/AndroidBinding** and **/src/iOSBinding** folders in this repo for the source for those. Those bindings are built separately and packaged into their own NuGet packages.

* In Visual Studio, right-click on the Movesense project, and then click **Edit Project File**
* Scroll down and find the **\<ItemGroup\>** elements that contain the \<PackageReference\> elements that pull in the Binding packages. Edit the version as appropriate to your needs:

   ```xml
     <ItemGroup Condition=" $(TargetFramework.StartsWith('MonoAndroid')) ">
    <Compile Include="Platforms\Android\**\*.cs" />
    <PackageReference Include="MovesenseBindingAndroid" Version="1.44.0" />
  </ItemGroup>

  <ItemGroup Condition=" $(TargetFramework.StartsWith('Xamarin.iOS')) ">
    <Compile Include="Platforms\iOS\**\*.cs" />
    <PackageReference Include="MovesenseBindingiOS" Version="1.44.0.1" />
  </ItemGroup>
   ```

### Building the NuGet package

Just build the project. The NuGet package is put in the /bin/debug or /bin/release folders depending on your configuration.

## Debugging and Unit Testing

Debugging a multi-target project is difficult, and one that is a Xamarin plugin is doubly so. To help with this, the solution contains full testing projects that enable this.

* **/Movesense.Test/MovesenseTestingAndroid** - this is a Xamarin Android class library that exists just so we can debug the Movesense.NET library code when running on Android devices. Nearly all the code in this project is just linked to the source in the main Movesense project.
* **/Movesense.Test/MovesenseTestingiOS** - this is a Xamarin iOS class library that exists just so we can debug the Movesense.NET library code when running on iOS devices. Nearly all the code in this project is just linked to the source in the main Movesense project.
* **/Movesense.Test/UnitTests/AndroidTestRunner** - To run unit tests on an Android device, set this project to be your startup project and then run it in the debugger. This references the *MovesenseTestingAndroid* project so you can debug right into the Movesense.NET library code. The actual unit tests are defined in the **MovesenseConnectionTestsAndroid** and **MovesenseTestsAndroid** class library projects.
* **/Movesense.Test/UnitTests/iOSTestRunner** - To run unit tests on an iOS device, set this project to be your startup project and then run it in the debugger. This references the *MovesenseTestingiOS* project so you can debug right into the Movesense.NET library code. The actual unit tests are defined in the **MovesenseConnectionTestsiOS** and **MovesenseTestsiOS** class library projects, although note that the source code files in those projects are actually linked to the files in the similar named Android projects, so you only need to write tests once and they are available on both Android and iOS.
* **/Movesense.Test/BasicApp** If you prefer to debug with a Xamarin Forms app as your client - this is it. The Android project in this references the *MovesenseTestingAndroid* project, and the iOS project the *MovesenseTestingiOS* project.
