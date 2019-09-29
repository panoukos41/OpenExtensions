# OpenExtensions

The goal is like the name, to have a library containing extensions open for everyone to contribute and use.
Make extensions for everything C# to make our life easier from WPF, UWP to Android, IOS, .Net Standard and .Net Core.

| Platform | Assemnly                | Nuget |
| -------- | ----------------------- | ----- |
| ALL      | OpenExtensions          | [![NuGet Package Icon](https://img.shields.io/nuget/v/OpenExtensions.svg)](https://www.nuget.org/packages/OpenExtensions) |
| UWP      | OpenExtensions.Uwp.UI   | [![NuGet Package Icon](https://img.shields.io/nuget/v/OpenExtensions.Uwp.UI)](https://www.nuget.org/packages/OpenExtensions.Uwp.UI) |
| Android  | OpenExtensions.Droid.UI | Coming soon! |

# Getting Started

To get started you will need:

1. Visual Studio 2019 version 16.3+
2. .Net Core 3.x
3. C# 7.3 (8 in the long future)

Compile and you should be ready to add your extensions, look into Contributing to see how extensions should be added.

***Things to consider starting with***
- Tests, i am not really familiar with testing in a library and any knowledge and guidance is appreciated.
- Samples.

# Contributing
Feedback and contributions are welcomed!

For information on how to contribute please see [Contributing to OpenExtensions](https://github.com/panoukos41/OpenExtensions/blob/master/CONTRIBUTING.md)

# Keep in mind

The library now contains extensions for the things i have worked on that being UWP and Xamarin.Android, it extends things from MvvmLight and contains code from [Prism.Windows](https://github.com/PrismLibrary/Prism/tree/7.1.0-pre4/Source/Windows10/Prism.Windows) branch since prism has abandoned UWP for now. One of my goals was to keep it compatible with UWP Anniversary update 14393 to use it on my app, of course this can change when the need comes to but I would really love the lowest platform version possible for each platform.

# License
The project is licenced under the [MIT License](https://en.wikipedia.org/wiki/MIT_License)

Licenses from other projects are under the licenses folder.