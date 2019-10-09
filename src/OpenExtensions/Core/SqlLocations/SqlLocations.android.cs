using System;

namespace OpenExtensions.Core
{
    public static partial class SqlLocations
    {
        /// <summary>
        /// 
        /// </summary>
        static string PlatformPath => Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    }
}
