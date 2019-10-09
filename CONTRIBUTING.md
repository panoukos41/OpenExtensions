# Contributing  and Issues
Everyone is welcome to help and extend this SDK type library, from bug reports to feature proposals, and code contributions.

GitHub issues is used to track bugs and features.
For all other bugs and general issues please file a [new issue](https://github.com/panoukos41/OpenExtensions/issues/new/choose) using the Bug Report template.

# Code contribution guidelines
Most extensions should be implemented like [Xamarin.Essentials](https://github.com/xamarin/Essentials) static partial classes that do their job.  
If it is necessary or makes more sense to implement an interface and then have the implementation in each platform separately the *Interface* should go in the Core Folder.  
The Interfaces in the Core folder should go at the same namespace and path as the implimentations but you must replace the Platform name with Core e.g: 
```c#
//Interface
namespace OpenExtensions.Core.Services
{
    public Interface IThemeService
    {
        // Common operations.
    }
}
// Class Uwp
using OpenExtensions.Core.Services;
namespace OpenExtensions.Uwp.Services
{
    public class ThemeService : IThemeService
    {
        // Platform code
    }
}
//Class Android
using OpenExtensions.Core.Services;
namespace OpenExtensions.Droid.Services
{
    public class ThemeService : IThemeService
    {
        // Platform code
    }
}
//Class Ios
using OpenExtensions.Core.Services;
namespace OpenExtensions.Ios.Services
{
    public class ThemeService : IThemeService
    {
        // Platform code
    }
}
```

# For OpenExtensions project

The main project is a multi-targeted project that has different file endings for each platform:
- .shared.cs for **shared** files (shared files compile on all platforms.) *Interfaces should go here for example!*
- .netstandard.cs for **.NET Standard** files
- .android.cs for **Android** files
- .uwp.cs for **Uwp** files
- .ios.cs for **Ios** files

*The ".shared.cs" and ".netstandard.cs" files are always excluded from compile for some reason as "Miscellaneous Files" so don't forget to undo the changes made to the .csproj file*.  
*For **IOS** files uncomment the part of the project that says IOS files and add an Ios folder if it's needed. It's commented for now since no actual extensions for ios exist*.

# For UI projects OpenExtensions.Uwp/Droid/Ios.UI
These projects must only target one platform (Uwp, Droid, Ios) and must contain resources or controls for that platform.

## For OpenExtensions.Uwp.UI
When you create a custom templated control create a xaml resource dictionary in the Themes folder then add it as a merged dictionary in Generic.xaml

## For OpenExtensions.Droid.UI
When you declare custom attributes the name of the xml file should be attrs_{your controls name here}.xml