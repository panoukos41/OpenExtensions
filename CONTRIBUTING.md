# Contributing

Everyone is welcome to help and extend this SDK type library, from bug reports to feature proposals, and code contributions.

# Issues

GitHub issues is used to track bugs and features.

For all other bugs and general issues please file a [new issue](https://github.com/panoukos41/OpenExtensions/issues/new/choose) using the Bug Report template.

# Code contribution guidelines

Contributions must go in develop first. Pull reaquests on master won't be accepted.

Most extensions should be implemented like [Xamarin.Essentials](https://github.com/xamarin/Essentials) static partial classes that do their job unless it is necessary or makes more sense to implement an interface and then have the implementation in each platform.

# OpenExtensions project

The main project is a multi-targeted project that has different file endings for each platform:

- .shared.cs for **shared** files (shared files compile on all platforms.)
- .netstandard.cs for **.NET Standard** files
- .android.cs for **Android** files
- .uwp.cs for **Uwp** files
- .ios.cs for **Ios** files

*The ".shared.cs" and ".netstandard.cs" files are always excluded from compile for some reason as "Miscellaneous Files" so don't forget to undo the changes made to the .csproj file*.

# OpenExtensions.Uwp/Droid/Ios.UI projects

These projects must only target one platform (Uwp, Droid, Ios) and must contain resources or controls for that platform.