## OpenExtensions

The goal is like the name, to have a library containing extensions open for everyone to contribute and use.
Make extensions for everything C# to make our life easier from WPF, UWP to Android, IOS, .Net Standard and .Net Core.

Nuget Packages:

[![NuGet Package Icon](https://img.shields.io/nuget/v/OpenExtensions.svg)](https://www.nuget.org/packages/OpenExtensions)

***Things to start with***
1. Tests need to be made, i am not really familiar with testing in a library and any knowledge and guidance is appreciated.
2. Samples need to be made too.

## Contribute

### Everyone is welcome to help and extend this SDK type library.
Contributions should go in develop first. When a good build from develop is ready a release branch the gitflow way should be made but if it's possible and develop is not a mess we can just merge to master.

Most extensions should be implemented like [Xamarin.Essentials](https://github.com/xamarin/Essentials) static partial classes that do their job unless it is necessary to implement an interface and then have the implementation in each platform, or it could just makes more sense.

Ui related extensions and controls should go to the appropriate project to have seperate nuget packages.

### Some things to keep in mind:

The library now contains extensions for the things i have worked on that being UWP and Xamarin.Android, it extends things from MvvmLight and contains code from [Prism.Windows](https://github.com/PrismLibrary/Prism/tree/7.1.0-pre4/Source/Windows10/Prism.Windows) branch since prism has abandoned UWP for now. One of my goals was to keep it compatible with UWP Anniversary update 14393 to use it on my app, of course this can change when the need comes to but I would really love the lowest platform version possible for each platform.

## License
The project is licenced under the [MIT License](https://en.wikipedia.org/wiki/MIT_License)
