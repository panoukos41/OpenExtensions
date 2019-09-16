using System;

namespace OpenExtensions.Core
{
    public static partial class SqlLocations
    {
        /// <summary>
        /// 
        /// </summary>
        static string PlatformPath => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db3");
    }
}
