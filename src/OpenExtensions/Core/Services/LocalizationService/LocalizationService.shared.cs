using System.Globalization;

namespace OpenExtensions.Core.Services
{
    /// <summary>
    /// A service to change culture faster and see current culture info.
    /// </summary>
    public static partial class LocalizationService
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
        public static void SetCulture(string cultureCode) => SetPlatformCulture(cultureCode);
    }
}
