using Android.Content;
using System.Threading.Tasks;

namespace OpenExtensions.Core.Services
{
    public static partial class ThemeService
    {
        private static ContextWrapper ContextWrapper;
        private static int DarkTheme;
        private static int LightTheme;
        private const string KEY = "Theme";

        /// <summary>
        /// Initialize the Android ThemeService for a specific Acitvity,
        /// Since this is a static class if the activity changes Initialize
        /// should be called again for that activity.
        /// </summary>
        /// <param name="contextWrapper">Your activity</param>
        /// <param name="darkTheme">Android Style Resource</param>
        /// <param name="lightTheme">Android Style Resource</param>
        public static void Initialize(ContextWrapper contextWrapper, int darkTheme, int lightTheme)
        {
            (ContextWrapper, DarkTheme, LightTheme) = (contextWrapper, darkTheme, lightTheme);
            if (GetCurrentTheme() == ThemeType.Dark)
                SetTheme(ThemeType.Dark);
            else
                SetTheme(ThemeType.Light);
        }

        static ThemeType GetPlatformCurrentTheme()
        {
            var pref = ContextWrapper.GetSharedPreferences("com.openExtensions.android.Theme", FileCreationMode.Private);
            return (ThemeType)pref.GetInt(KEY, 1);
        }

        static Task SetPlatformTheme(ThemeType theme)
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