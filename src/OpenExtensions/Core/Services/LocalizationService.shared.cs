using System.Globalization;

namespace OpenExtensions.Core.Services
{
    /// <summary>
    /// A service to change language faster and see current language info.
    /// </summary>
    public static class LocalizationService
    {
        /// <summary>
        /// Get the current culture object.
        /// </summary>
        public static CultureInfo Culture => CultureInfo.CurrentCulture;

        /// <summary>
        /// Get the TwoLetterISOLanguageName of the current culture object.
        /// </summary>
        public static string CultureName => Culture.TwoLetterISOLanguageName;

        /// <summary>
        /// Change language passing the two or four letter name of the language.
        /// </summary>
        public static void SetLanguage(string culture)
        {
            var cult = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = cult;
            CultureInfo.DefaultThreadCurrentUICulture = cult;
        }
    }
}
