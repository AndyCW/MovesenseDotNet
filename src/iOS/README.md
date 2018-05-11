# iOS Binding for Movesense

> _This post will be published on www.vodovnik.com soon_.

This document describes how to get the Movesense binding working for Xamarin, in iOS. The same approach can be taken when you need to include a (native) CocoaPods library into your Xamarin project.

We knew that to use Movesense in a Swift project, you have to install some CocoaPods containing the framework, and additionally, reference a _native_ library (libmds.a) which the pod uses for communication with the Movesense devices. To complicate things, the pod is actually located behind a private repository. While we have access to the repository, the steps to get that working as a reference of a Xamarin binding proved to not be documented anywhere.

## Sharpie

Most of the Xamarin documentation references a tool called [Objective Sharpie](https://docs.microsoft.com/en-us/xamarin/cross-platform/macios/binding/objective-sharpie/) which does indeed look like what we need. There's even some documentation available that covers the usage of Sharpie for [CocoaPods](https://docs.microsoft.com/en-us/xamarin/cross-platform/macios/binding/objective-sharpie/examples/cocoapod).

In our case, this was a bit of a problem as the CocoaPod is actually located behind a private repostitory and the documentation in no way explained how to get that working. Through trial and error, we established that sharpie init actually just creates a podfile, which meant that we could go on and fake it. We created a new `podfile` that looked something like this:

```bash
platform :ios, '11.3'
install! 'cocoapods', :integrate_targets => false

target 'ObjectiveSharpieIntegration' do
   use_frameworks!
end

# force the sub specs in the array below to use swift version 3.0
# Note: we assumed this is correct, and it was through a ton of trial and error
post_install do |installer|
    installer.pods_project.targets.each do |target|
            target.build_configurations.each do |config|
                config.build_settings['SWIFT_VERSION'] = '3.0'
        end
    end
end

pod 'Movesense', :git => 'location of the private repository'
```

From this point we ended up manually running `pod install`. After that completed, and installed all the references, we could finally run `sharpie pod bind` which took the state of the CocoaPods and created the _framework_ files we needed to reference.

From there on, we continued to follow the [documentation](https://docs.microsoft.com/en-us/xamarin/ios/platform/binding-objective-c/walkthrough?tabs=vsmac#Create_a_Xamarin.iOS_Binding_Project).

## Creating the Binding Project

The next step was to start creating the binding project in Xamarin, that we'll later be packaging up as a NuGet project. This is the _library_ that will be consumed by applications developed in C#. 

https://github.com/mxcl/PromiseKit/issues/722 — how we got it working

Library is a C++ (set a flag), add -lz (include Z library)

make sure apple account is signed in and provisioning profiles are downloaded

disable Incremental builds in Build options and Device specific builsa

Make sure Bluetooth LE is set in plist


Add all required NuGet packages for Swift


——

xcrun 