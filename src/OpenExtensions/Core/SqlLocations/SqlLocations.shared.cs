namespace OpenExtensions.Core
{
    /// <summary>
    /// Get the location for each platform. 
    /// The correct path is returned based on the platform.
    /// </summary>
    public static partial class SqlLocations
    {
        /// <summary>
        /// The path to use to create an sqlite database.
        /// </summary>
        public static string Path => PlatformPath;

        /// <summary>
        /// Returns the <see cref="Path"/> combined with the dbName, the name should also inlcude the file extension
        /// eg: db.sqlite or mydb.db3
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static string PathWithName(string dbName)
        {
            return System.IO.Path.Combine(Path, dbName);
        }
    }
}
