using Windows.Storage;

namespace OpenExtensions.Core
{
    public static partial class SqlLocations
    {
        /// <summary>
        /// 
        /// </summary>
        static string PlatformPath => System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite");
    }
}
