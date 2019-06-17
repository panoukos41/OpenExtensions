using OpenExtensions.Core.Services;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace OpenExtensions.UWP.Services
{
    /// <summary>
    /// Service to handle Light and Dark theme in UWP apps.
    /// </summary>
    public class ThemeService : IThemeService
    {
        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private const string KEY = "OpenExtensions_User_RequestedTheme";

        /// <summary>
        /// Get curent theme, <see cref="ThemeType"/> has the same integer 
        /// values as <see cref="ElementTheme"/>
        /// </summary>
        public ThemeType GetCurrentTheme()
        {
            var value = localSettings.Values[KEY];
            if (value == null)
            {
                localSettings.Values[KEY] = (int)ThemeType.Default;
                return ThemeType.Default;
            }
            return (ThemeType)value;
        }

        /// <summary>
        /// Set the new theme.
        /// </summary>
        /// <param name="theme"></param>
        public Task SetTheme(ThemeType theme)
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
