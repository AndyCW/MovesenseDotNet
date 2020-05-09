# Movesense Android Binding

This folder contains the implementation of the Xamarin bindings project that wraps the native Android MDSlib.aar library.

The output from this project has been published on NuGet, but you do not need to reference that package directly in your projects - it is added as a dependency of Plugin.Movesense.

## How to build the MovesenseBindingAndroid NuGet package

The output from this solution is packaged in a NuGet package **MovesenseBindingAndroid.*version*.nupkg**. These are the steps to build this package:

1. Get the latest version of the Mdslib libraries. These are published by Suunto at <https://bitbucket.org/suunto/movesense-mobile-lib/>. You will find the latest version of the Android **mdslib** in the movesense-mobile-lib/android/Movesense folder. For example, *mdslib-1.44.0(1)-release.aar*. 

1. Copy the Android mdslib and paste it into the **/Jars** folder in this project. Add it to the project.

1. Set the following File properties:
   * **Build Action:** LibraryProjectZip

1. Delete the previous version of **mdslib-*version*.aar** from the project.

1. Now build the project and hope...
   * If you just get warnings, then you're probably OK.
   * If you get errors, then more significant surgery is required. 

1. If you get errors, then the Binding project needs some more work. Read [Binding an .AAR"](https://docs.microsoft.com/xamarin/android/platform/binding-java-library/binding-an-aar) in the Xamarin documentation for more information.
   * You might need to add or update dependencies, such as *rxandroid-2.1.1.jar* or a new version of mdslib may intrioduce a new native Android library dependency. You can ask Movesense developer support for information on this. Whichever libraries are required, you have to get the jar file version. One of the best sources for jar files is the [Maven Central Repository](https://search.maven.org/).
   * You might also need to edit the **/Transforms/Metadata.xml** file, but only do this if you know what you are doing. Sometimes if you are getting a Build error for the binding project, you can just exclude the problematic methods if you think that they are not going to be called from the Movesense.net library - a fair bet since the public methods on the binding assembly are well known by now and unlikely to change.
   * Anyway - good luck!

1. Assuming it builds OK, you can package it at the command line as follows (substituting the desired version number):

`nuget pack -Version 0.0.1-beta -Properties Configuration=Release`

1. The official version is pushed to nuget.org at this point, but you can save the NuGet in a local folder for building your own custom version of Movesense.NET.

1. **Next Steps:**
   1. Build the [iOS binding](../../iOS/) project
   1. Build the [Movesense.NET](../../NuGet) library.