using Android.Content;
using System.Threading.Tasks;

namespace OpenExtensions.Droid.Services
{
    /// <summary>
    /// A service to easily register one light and one dark theme to switch between them.
    /// </summary>
    public class ThemeService
    {
        private readonly ContextWrapper ContextWrapper;
        private readonly int DarkTheme;
        private readonly int LightTheme;

        /// <summary>
        /// </summary>
        public enum Theme
        {
            /// <summary>
            /// </summary>
            Dark = 0,

            /// <summary>
            /// </summary>
            Light = 1
        }

        /// <summary>
        /// Initialize a new instance of the ThemeService
        /// </summary>
        /// <param name="contextWrapper">Your activity</param>
        /// <param name="darkTheme">Android Style Resource</param>
        /// <param name="lightTheme">Android Style Resource</param>
        public ThemeService(ContextWrapper contextWrapper, int darkTheme, int lightTheme)
        {
            (ContextWrapper, DarkTheme, LightTheme) = (contextWrapper, darkTheme, lightTheme);
            if (IsDarkTheme())
                SetTheme(Theme.Dark);
            else
                SetTheme(Theme.Light);
        }

        /// <summary>
        /// Checks if the current theme is the dark variant.
        /// </summary>
        /// <returns></returns>
        public bool IsDarkTheme()
        {
            var pref = ContextWrapper.GetSharedPreferences("com.openExtensions.android.Theme", FileCreationMode.Private);
            if (pref.GetInt("Theme", 1) == 0)
                return true;
            return false;

        }

        /// <summary>
        /// Set the choosen theme.
        /// </summary>
        /// <param name="theme"></param>
        public Task SetTheme(Theme theme)
        {
            var pref = ContextWrapper.GetSharedPreferences("com.openExtensions.android.Theme", FileCreationMode.Private).Edit();
            if (theme == Theme.Dark)
            {
                ContextWrapper.SetTheme(DarkTheme);
                pref.PutInt("Theme", 0);
            }
            else
            {
                ContextWrapper.SetTheme(LightTheme);
                pref.PutInt("Theme", 1);
            }
            pref.Apply();
            return Task.CompletedTask;
        }
    }
}