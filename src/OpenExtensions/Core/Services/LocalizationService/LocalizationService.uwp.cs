using System.Globalization;
using Windows.Globalization;

namespace OpenExtensions.Core.Services
{
    public static partial class LocalizationService
    {
        static void SetPlatformCulture(string cultureCode)
        {
            ApplicationLanguages.PrimaryLanguageOverride = cultureCode;
            var cult = new CultureInfo(cultureCode);
            CultureInfo.DefaultThreadCurrentCulture = cult;
            CultureInfo.DefaultThreadCurrentUICulture = cult;
        }
    }
}
