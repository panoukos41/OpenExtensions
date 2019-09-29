namespace OpenExtensions.Core
{
    /// <summary>
    /// Get the location for each platform. 
    /// The correct path is returned based on the platform.
    /// </summary>
    public static partial class SqlLocations
    {
        /// <summary>
        /// The path includes the sqlite file.
        /// </summary>
        public static string Path => PlatformPath;
    }
}
