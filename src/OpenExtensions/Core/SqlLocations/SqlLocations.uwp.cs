using Windows.Storage;

namespace OpenExtensions.Core
{
    public static partial class SqlLocations
    {
        /// <summary>
        /// 
        /// </summary>
        static string PlatformPath => ApplicationData.Current.LocalFolder.Path;
    }
}
