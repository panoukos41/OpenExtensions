using System.Threading.Tasks;

namespace OpenExtensions.Core.Services
{
    /// <summary>
    /// A service to set the device theme, and determine which theme is active.
    /// For Android call Initialize to provide your own Light and Dark themes. 
    /// The default theme for Android is Light.
    /// For Uwp the default theme is the System Default from user Settings.
    /// </summary>
    public static partial class ThemeService
    {
        /// <summary>
        /// Get the current theme.
        /// </summary>
        /// <returns></returns>
        public static ThemeType GetCurrentTheme() => GetPlatformCurrentTheme();

        /// <summary>
        /// Set the choosen theme.
        /// </summary>
        /// <param name="theme"></param>
        public static Task SetTheme(ThemeType theme) => SetPlatformTheme(theme);
    }
}
