using System.Threading.Tasks;

namespace OpenExtensions.Core.Services
{
    /// <summary>
    /// The type of the theme used in <see cref="IThemeService"/>
    /// Enum integer values are the same as UWP ElementTheme values.
    /// </summary>
    public enum ThemeType
    {
        /// <summary>
        /// </summary>
        Default = 0,

        /// <summary>
        /// </summary>
        Light = 1,

        /// <summary>
        /// </summary>
        Dark = 2
    }

    /// <summary>
    /// A service to set the devices theme, and determine which theme is active.
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// Get the current theme.
        /// </summary>
        /// <returns></returns>
        ThemeType GetCurrentTheme();

        /// <summary>
        /// Set the choosen theme.
        /// </summary>
        /// <param name="theme"></param>
        Task SetTheme(ThemeType theme);
    }
}
