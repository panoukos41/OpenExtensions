using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace OpenExtensions.Core.Services
{
    public static partial class ThemeService
    {
        private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private const string KEY = "OpenExtensions_User_RequestedTheme";

        static ThemeType GetPlatformCurrentTheme()
        {
            var value = localSettings.Values[KEY];
            if (value == null)
            {
                localSettings.Values[KEY] = (int)ThemeType.Default;
                return ThemeType.Default;
            }
            return (ThemeType)value;
        }

        static Task SetPlatformTheme(ThemeType theme)
        {
            localSettings.Values[KEY] = (int)theme;
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = (ElementTheme)theme;
            }
            return Task.CompletedTask;
        }
    }
}
