using GalaSoft.MvvmLight.Views;

namespace OpenExtensions.Droid.FragmentNavigation
{
    /// <summary>
    /// A navigation service that's used as a fragment
    /// to use with fragments extending the <see cref="INavigationService"/>.
    /// </summary>
    public interface IFragmentNavigationService : INavigationService
    {
        /// <summary>
        /// A bool indicating if the fragment service was created. 
        /// this is necessary since android seems to not update the fragment flag.
        /// </summary>
        bool IsCreated { get; }

        /// <summary>
        /// A bool indicatting if the service can go back.
        /// </summary>
        /// <returns></returns>
        bool CanGoback();
    }
}