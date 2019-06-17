using Android.Content;
using OpenExtensions.Core.Services;
using System.Threading.Tasks;

namespace OpenExtensions.Droid.Services
{
    /// <summary>
    /// A service to easily register one light and one dark theme to switch between them.
    /// </summary>
    public class ThemeService : IThemeService
    {
        private readonly ContextWrapper ContextWrapper;
        private readonly int DarkTheme;
        private readonly int LightTheme;
        private const string KEY = "Theme";

        /// <summary>
        /// Initialize a new instance of the ThemeService
        /// </summary>
        /// <param name="contextWrapper">Your activity</param>
        /// <param name="darkTheme">Android Style Resource</param>
        /// <param name="lightTheme">Android Style Resource</param>
        public ThemeService(ContextWrapper contextWrapper, int darkTheme, int lightTheme)
        {
            (ContextWrapper, DarkTheme, LightTheme) = (contextWrapper, darkTheme, lightTheme);
            if (GetCurrentTheme() == ThemeType.Dark)
                SetTheme(ThemeType.Dark);
            else
                SetTheme(ThemeType.Light);
        }

        /// <summary>
        /// Get the current theme.
        /// </summary>
        /// <returns></returns>
        public ThemeType GetCurrentTheme()
        {
            var pref = ContextWrapper.GetSharedPreferences("com.openExtensions.android.Theme", FileCreationMode.Private);
            return (ThemeType)pref.GetInt(KEY, 1);
        }

        /// <summary>
        /// Set the choosen theme, for android Default will result in Light.
        /// </summary>
        /// <param name="theme"></param>
        public Task SetTheme(ThemeType theme)
        {
            var pref = ContextWrapper.GetSharedPreferences("com.openExtensions.android.Theme", FileCreationMode.Private).Edit();
            if (theme == ThemeType.Dark)
            {
                ContextWrapper.SetTheme(DarkTheme);
                pref.PutInt("Theme", (int)ThemeType.Dark);
            }
            else
            {
                ContextWrapper.SetTheme(LightTheme);
                pref.PutInt("Theme", (int)ThemeType.Light);
            }
            pref.Apply();
            return Task.CompletedTask;
        }
    }
}