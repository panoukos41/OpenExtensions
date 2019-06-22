using Android.Support.V4.App;
using Java.Lang;

namespace OpenExtensions.Droid.Adapters
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFragmentPagerAdapterEx
    {
        /// <summary>
        /// Returns a fragment for the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Fragment GetPage(int index);

        /// <summary>
        /// Returns a title for the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string GetPageTitle(int index);
    }
}
