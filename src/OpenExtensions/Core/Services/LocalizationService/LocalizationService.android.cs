using System.Globalization;

namespace OpenExtensions.Core.Services
{
    public static partial class LocalizationService
    {
        static void SetPlatformCulture(string cultureCode)
        {
            var cult = new CultureInfo(cultureCode);
            CultureInfo.DefaultThreadCurrentCulture = cult;
            CultureInfo.DefaultThreadCurrentUICulture = cult;
        }
    }
}
